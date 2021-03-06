﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MensMorris.Engine
{
    /// <summary>
    /// Represents a match of Mens Morris
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Notifies about the completion of the match
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Gets the settings
        /// </summary>
        public Settings Settings { get; private set; }

        // A list of all positions on the board
        private List<BoardPosition> Board;

        // Both slots in an array to iterate over
        private Slot[] Slots;

        /// <summary>
        /// Gets the current game phase (Placing or Moving)
        /// </summary>
        public GamePhase Phase { get; private set; }

        /// <summary>
        /// Gets or sets the wait time before each round
        /// </summary>
        public TimeSpan WaitTime { get; set; }

        /// <summary>
        /// Creates a new Mens Morris match
        /// </summary>
        /// <param name="settings">The match settings</param>
        /// <param name="firstPlayer">The player for the first slot</param>
        /// <param name="secondPlayer">The player for the second slot</param>
        public Match(Settings settings, IPlayer firstPlayer, IPlayer secondPlayer)
        {
            // Set default round sleep time to one 1ms
            this.WaitTime = new TimeSpan(0, 0, 0, 0, 1);
            // Set the match settings
            this.Settings = settings;
            // Set initial phase to Placing
            this.Phase = GamePhase.PlacingPhase;
            // Create player slots
            this.Slots = new Slot[2];
            this.Slots[0] = new Slot(this, firstPlayer, 0);
            this.Slots[1] = new Slot(this, secondPlayer, 1);
            // Build the board
            this.Board = new List<BoardPosition>();
            // Arrays to remember middle and corner positions for connections between the rings
            BoardPosition[] middles = new BoardPosition[4];
            BoardPosition[] corners = new BoardPosition[4];
            if (this.Settings.CenterPoint)
            {
                // Create a point in the center of the board
                BoardPosition center = new BoardPosition(this, 0, 0);
                // Setup the connection with each border and each corner point
                for (int i = 0; i <= 3; i++)
                {
                    middles[i] = center;
                    corners[i] = center;
                }
                this.Board.Add(center);
            }
            for (int ring = 1; ring <= this.Settings.RingCount; ring++) // For each ring
            {
                Direction direction = DirectionHelpers.ForSide(0);
                // Create and remember first position
                BoardPosition first = new BoardPosition(this, ring, 0);
                // First position is a corner position
                if (this.Settings.ConnectCorners)
                {
                    // Connect corner positions
                    if (corners[0] != null)
                    {
                        corners[0].SetNeighbor(direction.Turn(5 /* 225° */), first);
                        first.SetNeighbor(direction.Turn(5 /* 225° */).Opposite(), corners[0]);
                    }
                    // Remember corner position
                    corners[0] = first;
                }
                this.Board.Add(first);
                // Remember previous position
                BoardPosition previous = first;
                for (int sideNumber = 0; sideNumber <= 3; sideNumber++) // For each side
                {
                    // Get the onwards direction for the ring side
                    direction = DirectionHelpers.ForSide(sideNumber);
                    for (int positionNumber = 1; positionNumber <= (sideNumber < 3 ? 2 : 1); positionNumber++) // For each position
                    {
                        // Create a new position
                        BoardPosition position = new BoardPosition(this, ring, sideNumber * 2 + positionNumber);
                        // Set neighbors
                        previous.SetNeighbor(direction, position);
                        position.SetNeighbor(direction.Opposite(), previous);
                        if (positionNumber == 1) // Middle position
                        {
                            // Connect middle positions
                            if (middles[sideNumber] != null)
                            {
                                middles[sideNumber].SetNeighbor(direction.Turn(6 /* 270° */), position);
                                position.SetNeighbor(direction.Turn(6 /* 270° */).Opposite(), middles[sideNumber]);
                            }
                            // Remember middle position
                            middles[sideNumber] = position;
                            // Prepare center cross if activated
                            if (this.Settings.CenterCross && ring == 1 && sideNumber <= 1)
                            {
                                middles[sideNumber + 2] = position;
                            }
                        }
                        if (this.Settings.ConnectCorners && positionNumber == 2) // Corner position
                        {
                            // Connect corner positions
                            if (corners[sideNumber + 1] != null)
                            {
                                corners[sideNumber + 1].SetNeighbor(direction.Turn(7 /* 315° */), position);
                                position.SetNeighbor(direction.Turn(7 /* 315° */).Opposite(), corners[sideNumber + 1]);
                            }
                            // Remember corner position
                            corners[sideNumber + 1] = position;
                        }
                        // Add position to board
                        this.Board.Add(position);
                        previous = position;
                    }
                }
                // Connect the first and the last position of a ring
                previous.SetNeighbor(direction, first);
                first.SetNeighbor(direction.Opposite(), previous);
            }
        }

        private bool shouldStop = false;

        /// <summary>
        /// Start the match
        /// </summary>
        public void Start()
        {
            // Start in a new background thread
            Thread matchThread = new Thread(this.MatchLoop);
            matchThread.IsBackground = true;
            matchThread.Start();
        }

        /// <summary>
        /// Stop the match
        /// </summary>
        public void Stop()
        {
            // Reset the match loop condition
            this.shouldStop = true;
        }

        /// <summary>
        /// The match loop
        /// </summary>
        private void MatchLoop()
        {
            // Start with first slot
            Slot currentSlot = this.GetSlot(0);
            // Required for kick condition check
            Tile usedTile = null;
            // Required for phase transition check
            int placingCounter = 1;
            while (!this.IsMatchDone() && !this.shouldStop)
            {
                // Set OnTurn state for current slot
                currentSlot.SetOnTurn(true);
                // Wait the configured timespan
                Thread.Sleep(this.WaitTime);
                switch (this.Phase)
                {
                    case GamePhase.PlacingPhase:
                        // Collect possible actions and let the player select one
                        PlaceAction placeAction = currentSlot.HandlePlaceAction();
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        if (placeAction != null) placeAction.Tile.To(placeAction.Target);
                        // Remember the used tile for kick condition
                        usedTile = (placeAction != null) ? placeAction.Tile : null;
                        // Check for phase transition
                        if (placingCounter++ == this.Settings.TilesPerSlot * 2) this.Phase = GamePhase.MovingPhase;
                        break;
                    case GamePhase.MovingPhase:
                        // Collect possible actions and let the player select one
                        MoveAction moveAction = currentSlot.HandleMoveAction();
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        if (moveAction != null) moveAction.Tile.To(moveAction.Target);
                        // Remember the used tile for kick condition
                        usedTile = (moveAction != null) ? moveAction.Tile : null;
                        break;
                }
                // Check for the used tile
                if (usedTile != null)
                {
                    // Check for a formed mill => allow kick action
                    if (usedTile.FormsMill())
                    {
                        // Collect possible actions and let the player select one
                        KickAction kickAction = currentSlot.HandleKickAction();
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        kickAction.OpponentTile.To(null);
                    }
                }
                else
                {
                    // This means the slot looses the game
                    currentSlot.NoPossibleMove = true;
                }
                // Reset OnTurn state for current slot
                currentSlot.SetOnTurn(false);
                // Switch the current slot
                currentSlot = currentSlot.GetOpponent();
            }
            // Inform about the finished game
            this.Finished?.BeginInvoke(this, EventArgs.Empty, this.Finished.EndInvoke, null);
        }

        /// <summary>
        /// Gets a list of all board positions
        /// </summary>
        /// <returns></returns>
        public List<BoardPosition> GetBoard()
        {
            // Copy list to prevent manipulation
            return this.Board.ToList();
        }

        /// <summary>
        /// Simulates an action
        /// </summary>
        /// <remarks>
        /// The action simulated via this function can be reverted at any time by using Revert or the auto-revert functionality:
        /// When this function is called in the beginning of a 'using' scope, the Revert function is called automatically after leaving the using scope.
        /// </remarks>
        /// <param name="action">The action to simulate</param>
        /// <returns>
        /// An auto-revert object for a 'using' scope
        /// </returns>
        public AutoRevert SimulateAction(BaseAction action)
        {
            if (action is PlaceAction)
            {
                PlaceAction placeAction = action as PlaceAction;
                placeAction.Tile.SimulateTo(placeAction.Target);
            }
            if (action is MoveAction)
            {
                MoveAction moveAction = action as MoveAction;
                moveAction.Tile.SimulateTo(moveAction.Target);
            }
            if (action is KickAction)
            {
                KickAction kickAction = action as KickAction;
                kickAction.OpponentTile.SimulateTo(null);
            }
            // Return the auto-revert object
            return new AutoRevert(this);
        }

        /// <summary>
        /// Reverts any simulated action
        /// </summary>
        public void Revert()
        {
            // Revert each board position
            this.Board.ForEach(position => position.Revert());
            // Revert each tile
            this.GetTiles().ForEach(tile => tile.Revert());
        }

        /// <summary>
        /// Gets a list of the slots participating in this match
        /// </summary>
        public List<Slot> GetSlots()
        {
            return this.Slots.ToList();
        }

        /// <summary>
        /// Gets the slot of a specific number
        /// </summary>
        public Slot GetSlot(int number)
        {
            return this.Slots[number];
        }

        /// <summary>
        /// Gets all tiles used in this match
        /// </summary>
        public List<Tile> GetTiles()
        {
            return this.Slots
                .Select(slot => slot.GetTiles())
                .Aggregate<IEnumerable<Tile>>((tiles1, tiles2) => tiles1.Union(tiles2))
                .ToList();
        }

        /// <summary>
        /// Gets all empty positions on the board of this match
        /// </summary>
        public List<BoardPosition> GetEmptyPositions()
        {
            return this.Board
                .Where(pos => pos.Current == null)
                .ToList();
        }

        /// <summary>
        /// Indicates whether the match is done and one slot has won the game
        /// </summary>
        public bool IsMatchDone()
        {
            return this.Phase == GamePhase.MovingPhase && this.Slots.Any(slot => slot.HasLost);
        }

        /// <summary>
        /// Determines the slot which won the match when it is finished
        /// </summary>
        /// <returns>The winning slot or null, when the match is not done</returns>
        public Slot GetWinnerSlot()
        {
            return this.IsMatchDone() ? this.Slots.Single(slot => !slot.HasLost) : null;
        }

    }

    /// <summary>
    /// Provides the auto-revert functionality for matches with help of the 'using' keyword
    /// </summary>
    /// <remarks>
    /// .NET offers the possibility to free ressources automatically with the 'using' keyword and the IDisposable interface.
    /// When a function returns an IDisposable and it is called in the beginning of a 'using' scope, the Dispose function of the IDisposable is called automatically after using the scope.
    /// This is used in this case to auto-revert the changes made by the SimulateAction function of a match.
    /// </remarks>
    public class AutoRevert : IDisposable
    {
        private Match SimulatedMatch;

        /// <summary>
        /// Creates a new auto revert object for a given match
        /// </summary>
        /// <param name="simulatedMatch">The simulated match</param>
        public AutoRevert(Match simulatedMatch)
        {
            this.SimulatedMatch = simulatedMatch;
        }

        /// <summary>
        /// Reverts any changes made to the assigned simulated match
        /// </summary>
        /// <remarks>
        /// This is called automatically at the end of the 'using' scope.
        /// </remarks>
        public void Dispose()
        {
            this.SimulatedMatch.Revert();
        }
    }
}
