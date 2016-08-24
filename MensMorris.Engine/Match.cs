using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MensMorris.Engine
{
    public class Match
    {
        public event EventHandler Finished;

        private List<BoardPosition> Board;

        private Slot[] Slots;

        public GamePhase Phase { get; private set; }

        public TimeSpan WaitTime { get; set; }

        public Match(IPlayer firstPlayer, IPlayer secondPlayer)
        {
            // Set default round sleep time to one 1ms
            this.WaitTime = new TimeSpan(0, 0, 0, 0, 500);
            // Set initial phase
            this.Phase = GamePhase.PlacingPhase;
            // Create player slots
            this.Slots = new Slot[2];
            this.Slots[0] = new Slot(firstPlayer, 0);
            this.Slots[1] = new Slot(secondPlayer, 1);
            // Build the board
            this.Board = new List<BoardPosition>();
            BoardPosition[] middle = new BoardPosition[4];
            for (int ring = 1; ring <= 3; ring++) // For each ring
            {
                Direction direction = Direction.Left;
                // Create and remember first position
                BoardPosition first = new BoardPosition(ring, 0);
                this.Board.Add(first);
                // Remember previous position
                BoardPosition previous = first;
                for (int a = 0; a <= 3; a++) // For each side
                {
                    direction = (Direction)a;
                    for (int b = 1; b <= (a < 3 ? 2 : 1); b++) // For each position
                    {
                        // Create a new position
                        BoardPosition position = new BoardPosition(ring, a * 2 + b);
                        // Set neighbors
                        previous.SetNeighbor(direction, position);
                        position.SetNeighbor(direction.Opposite(), previous);
                        if (b == 1)
                        {
                            // Connect middle positions
                            if (middle[a] != null)
                            {
                                middle[a].SetNeighbor(direction.Next(), position);
                                position.SetNeighbor(direction.Next().Opposite(), middle[a]);
                            }
                            // Remember middle position
                            middle[a] = position;
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
            int currentSlotId = 0;
            Slot currentSlot = null;
            Tile usedTile = null;
            int placingCounter = 1;
            while (!this.IsGameDone() && !this.shouldStop)
            {
                // Assign the current slot
                currentSlot = this.Slots[currentSlotId];
                // Set OnTurn state for current slot
                currentSlot.SetOnTurn(true);
                // Wait the configured timespan
                Thread.Sleep(this.WaitTime);
                switch (this.Phase)
                {
                    case GamePhase.PlacingPhase:
                        // Perform the place action
                        PlaceAction place = currentSlot.DoPlaceAction(this);
                        usedTile = (place != null) ? place.ToPlace : null;
                        // Check for phase transition
                        if (placingCounter++ == 18) this.Phase = GamePhase.MovingPhase;
                        break;
                    case GamePhase.MovingPhase:
                        // Perform the move action
                        MoveAction move = currentSlot.DoMoveAction(this);
                        usedTile = (move != null) ? move.ToMove : null;
                        break;
                }
                if (usedTile != null)
                {
                    if (usedTile.FormsMill())
                    {
                        KickAction kick = currentSlot.DoKickAction(this);
                    }
                }
                else
                {
                    currentSlot.NoPossibleMove = true;
                }
                // Reset OnTurn state for current slot
                currentSlot.SetOnTurn(false);
                // Switch the player to either first or second player
                currentSlotId = (currentSlotId == 0) ? 1 : 0;
            }
            // Inform about the finished game
            this.Finished?.BeginInvoke(this, EventArgs.Empty, this.Finished.EndInvoke, null);
        }

        public List<BoardPosition> GetBoard()
        {
            return this.Board.ToList();
        }

        public Match SimulateAction(BaseAction action)
        {
            return this;
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
}
