using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    public class BoardPosition
    {
        public int Ring { get; private set; }
        public int Number { get; private set; }

        public Match Match { get; private set; }

        protected Dictionary<Direction, BoardPosition> Neighbors;

        public Tile Current { get; protected set; }

        public bool IsFree
        {
            get
            {
                return this.Current == null;
            }
        }

        public BoardPosition(Match match, int ring, int number)
        {
            this.Match = match;
            this.Ring = ring;
            this.Number = number;
            this.Neighbors = new Dictionary<Direction, BoardPosition>();
            this.Current = null;
        }

        internal void SetNeighbor(Direction direction, BoardPosition position)
        {
            this.Neighbors.Add(direction, position);
        }

        internal void Put(Tile tile)
        {
            this.Current = tile;
        }

        public BoardPosition Neighbor(Direction direction)
        {
            BoardPosition result;
            return this.Neighbors.TryGetValue(direction, out result) ? result : null;
        }

        public List<BoardPosition> GetNeighbors()
        {
            return this.Neighbors.Values.ToList();
        }

        #region | Simulation |

        private BoardPositionSnapshot Snapshot;

        public bool IsSimulated
        {
            get
            {
                return this.Snapshot != null;
            }
        }

        internal void SimulatePut(Tile simulatedTile)
        {
            if (this.Snapshot == null) this.Snapshot = new BoardPositionSnapshot(this.Current);
            this.Current = simulatedTile;
        }

        internal void Revert()
        {
            if (this.Snapshot != null)
            {
                this.Current = this.Snapshot.OriginalCurrent;
                this.Snapshot = null;
            }
        }

        #endregion // Simulation
    }

    public class BoardPositionSnapshot
    {
        public Tile OriginalCurrent { get; private set; }

        public BoardPositionSnapshot(Tile original)
        {
            this.OriginalCurrent = original;
        }
    }

}
