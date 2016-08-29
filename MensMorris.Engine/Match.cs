using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MensMorris.Engine
{
    public class Match
    {
        public event EventHandler Finished;

        public Settings Settings { get; private set; }

        private List<BoardPosition> Board;

        private Slot[] Slots;

        public GamePhase Phase { get; private set; }

        public TimeSpan WaitTime { get; set; }

        public Match(Settings settings, IPlayer firstPlayer, IPlayer secondPlayer)
        {
            // Set default round sleep time to one 1ms
            this.WaitTime = new TimeSpan(0, 0, 0, 0, 500);
            // Set the game settings
            this.Settings = settings;
            // Set initial phase
            this.Phase = GamePhase.PlacingPhase;
            // Create player slots
            this.Slots = new Slot[2];
            this.Slots[0] = new Slot(this, firstPlayer, 0);
            this.Slots[1] = new Slot(this, secondPlayer, 1);
            // Build the board
            this.Board = new List<BoardPosition>();
            BoardPosition[] middles = new BoardPosition[4];
            BoardPosition[] corners = new BoardPosition[4];
            if (this.Settings.CenterPoint)
            {
                BoardPosition center = new BoardPosition(this, 0, 0);
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
                    // Remember middle position
                    corners[0] = first;
                }
                this.Board.Add(first);
                // Remember previous position
                BoardPosition previous = first;
                for (int sideNumber = 0; sideNumber <= 3; sideNumber++) // For each side
                {
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
                // Connect the first and the last position
                previous.SetNeighbor(direction, first);
                first.SetNeighbor(direction.Opposite(), previous);
            }
        }

        private bool shouldStop = false;

        public void Start()
        {
            (new Thread(() => this.GameLoop())).Start();
        }

        public void Stop()
        {
            this.shouldStop = true;
        }

        private void GameLoop()
        {
            // Start with first slot
            Slot currentSlot = this.GetSlot(0);
            // Required for kick condition check
            Tile usedTile = null;
            int placingCounter = 1;
            while (!this.IsGameDone() && !this.shouldStop)
            {
                // Set OnTurn state for current slot
                currentSlot.SetOnTurn(true);
                // Wait the configured timespan
                Thread.Sleep(this.WaitTime);
                switch (this.Phase)
                {
                    case GamePhase.PlacingPhase:
                        // Collect possible actions and let the player select one
                        PlaceAction placeAction = currentSlot.HandlePlaceAction(this);
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        placeAction.Tile.To(placeAction.Target);
                        // Remember the used tile for kick condition
                        usedTile = placeAction.Tile;
                        // Check for phase transition
                        if (placingCounter++ == this.Settings.TilesPerSlot * 2) this.Phase = GamePhase.MovingPhase;
                        break;
                    case GamePhase.MovingPhase:
                        // Collect possible actions and let the player select one
                        MoveAction moveAction = currentSlot.HandleMoveAction(this);
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        moveAction.Tile.To(moveAction.Target);
                        usedTile = (moveAction != null) ? moveAction.Tile : null;
                        break;
                }
                if (usedTile != null)
                {
                    if (usedTile.FormsMill())
                    {
                        // Collect possible actions and let the player select one
                        KickAction kickAction = currentSlot.HandleKickAction(this);
                        // Revert any simulation
                        this.Revert();
                        // Execute the action
                        kickAction.OpponentTile.To(null);
                    }
                }
                else
                {
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

        public List<BoardPosition> GetBoard()
        {
            // Copy list to prevent manipulation
            return this.Board.ToList();
        }

        public MatchRevert SimulateAction(BaseAction action)
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
            return new MatchRevert(this);
        }

        public void Revert()
        {
            this.Board.ForEach(position => position.Revert());
            this.GetTiles().ForEach(tile => tile.Revert());
        }

        public List<Slot> GetSlots()
        {
            return this.Slots.ToList();
        }

        public Slot GetSlot(int number)
        {
            return this.Slots[number];
        }

        public List<Tile> GetTiles()
        {
            return this.Slots
                .Select(slot => slot.GetTiles())
                .Aggregate<IEnumerable<Tile>>((tiles1, tiles2) => tiles1.Union(tiles2))
                .ToList();
        }

        public List<BoardPosition> GetEmptyPositions()
        {
            return this.Board
                .Where(pos => pos.Current == null)
                .ToList();
        }

        public bool IsGameDone()
        {
            return this.Phase == GamePhase.MovingPhase && this.Slots.Any(slot => slot.HasLost);
        }

        public Slot GetWinnerSlot()
        {
            return this.IsGameDone() ? this.Slots.Single(slot => !slot.HasLost) : null;
        }

    }

    public class MatchRevert : IDisposable
    {
        private Match SimulatedMatch;

        public MatchRevert(Match simulatedMatch)
        {
            this.SimulatedMatch = simulatedMatch;
        }

        public void Dispose()
        {
            this.SimulatedMatch.Revert();
        }
    }
}
