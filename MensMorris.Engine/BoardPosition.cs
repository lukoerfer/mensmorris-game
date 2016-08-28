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

        internal void SetCurrent(Tile tile)
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
    }

    public class SimulatedBoardPosition : BoardPosition
    {
        public BoardPosition Original { get; private set; }

        public SimulatedBoardPosition(BoardPosition orig) : base(orig.Match, orig.Ring, orig.Number)
        {
            this.Original = orig;
        }

        public void CopyNeighbors(Dictionary<BoardPosition, SimulatedBoardPosition> simulatedBoard)
        {
            foreach (Direction dir in DirectionHelpers.Enumerate())
            {
                BoardPosition originalNeighbor = this.Original.Neighbor(dir);
                if (originalNeighbor != null) this.SetNeighbor(dir, simulatedBoard[originalNeighbor]);
            }
        }
    }
}
