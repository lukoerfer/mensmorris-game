using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    public class BoardPosition
    {
        public int Ring { get; private set; }
        public int Number { get; private set; }

        private BoardPosition[] Neighbors;

        public Tile Current { get; private set; }

        public bool IsFree
        {
            get
            {
                return this.Current == null;
            }
        }

        public BoardPosition(int ring, int number)
        {
            this.Ring = ring;
            this.Number = number;
            this.Neighbors = new BoardPosition[4];
            this.Current = null;
        }

        internal void SetNeighbor(Direction direction, BoardPosition position)
        {
            this.Neighbors[(int)direction] = position;
        }

        internal void SetCurrent(Tile tile)
        {
            this.Current = tile;
        }

        public BoardPosition Neighbor(Direction direction)
        {
            return this.Neighbors[(int)direction];
        }

        public List<BoardPosition> GetNeighbors()
        {
            return this.Neighbors.Where(neighbor => neighbor != null).ToList();
        }

        
    }
}
