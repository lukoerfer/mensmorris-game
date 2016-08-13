using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class Match
    {
        private List<BoardPosition> board;

        public IEnumerable<BoardPosition> Board
        {
            get
            {
                return this.board.ToList();
            }
        }

        private Slot[] slots;

        public IEnumerable<Slot> Slots
        {
            get
            {
                return this.slots.ToList();
            }
        }

        public GamePhase Phase { get; private set; }

        public TimeSpan WaitTime { get; set; }

        public Match(IPlayer firstPlayer, IPlayer secondPlayer)
        {
            // Set default round sleep time to one 1ms
            this.WaitTime = new TimeSpan(0, 0, 0, 0, 20);
            // Set initial phase
            this.Phase = GamePhase.PlacingPhase;
            // Create player slots
            this.slots = new Slot[2];
            this.slots[0] = new Slot(firstPlayer, 0);
            this.slots[1] = new Slot(secondPlayer, 1);
            // Build the board
            this.board = new List<BoardPosition>();
            BoardPosition[] middle = new BoardPosition[4];
            for (int ring = 1; ring <= 3; ring++) // For each ring
            {
                Direction direction = Direction.Left;
                // Create and remember first position
                BoardPosition first = new BoardPosition(ring, 0);
                this.board.Add(first);
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
                        this.board.Add(position);
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
                // Wait the configured timespan
                Thread.Sleep(this.WaitTime);
                // Assign the current slot
                currentSlot = this.slots[currentSlotId];
                switch (this.Phase)
                {
                    case GamePhase.PlacingPhase:
                        // Perform the place action
                        PlaceAction place = currentSlot.DoPlaceAction(this);
                        usedTile = (place != null) ? place.PlacedTile : null;
                        // Check for phase transition
                        if (placingCounter++ == 18) this.Phase = GamePhase.MovingPhase;
                        break;
                    case GamePhase.MovingPhase:
                        // Perform the move action
                        MoveAction move = currentSlot.DoMoveAction(this);
                        usedTile = (move != null) ? move.MovingTile : null;
                        break;
                }
                if (usedTile != null)
                {
                    if (usedTile.DoesFormMill())
                    {
                        KickAction kick = currentSlot.DoKickAction(this);
                    }
                }
                else
                {
                    currentSlot.NoPossibleMove = true;
                }
                // Switch the player to either first or second player
                currentSlotId = (currentSlotId == 0) ? 1 : 0;
            }
        }

        public Slot GetSlot(int number)
        {
            return this.slots[number];
        }

        public IEnumerable<Tile> GetTiles()
        {
            return this.slots.Select(slot => slot.Tiles).Aggregate((tiles1, tiles2) => tiles1.Union(tiles2));
        }

        public IEnumerable<BoardPosition> GetEmptyPositions()
        {
            return this.board.Where(pos => pos.Current == null);
        }

        public bool IsGameDone()
        {
            return this.Phase == GamePhase.MovingPhase && this.slots.Any(slot => slot.HasLost);
        }

    }
}
