using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    public class Tile
    {
        public event EventHandler AtChanged;

        public Slot Owner { get; private set; }

        public BoardPosition At { get; protected set; }

        public Tile(Slot owner)
        {
            this.Owner = owner;
            this.At = null;
        }

        internal void GoTo(BoardPosition pos)
        {
            // Reset the previous board position
            if (this.At != null) this.At.SetCurrent(null);
            this.At = pos;
            // Set the new board position
            if (this.At != null) this.At.SetCurrent(this);
            this.AtChanged?.BeginInvoke(this, EventArgs.Empty, this.AtChanged.EndInvoke, null);
        }

        public bool FormsMill()
        {
            if (this.At != null)
            {
                return DirectionHelpers.Enumerate().Any(dir => this.Owner.DoesOwn(this.At.Neighbor(dir)) &&
                    (this.Owner.DoesOwn(this.At.Neighbor(dir.Opposite())) || this.Owner.DoesOwn(this.At.Neighbor(dir).Neighbor(dir))));
            }
            else
            {
                return false;
            }
        }
    }

    public class SimulatedTile : Tile
    {
        public Tile Original { get; private set; }

        public SimulatedTile(Tile orig) : base(orig.Owner)
        {
            this.Original = orig;
        }

        public void CopyAt(Dictionary<BoardPosition, SimulatedBoardPosition> simulatedBoard)
        {
            if (this.Original.At != null) this.GoTo(simulatedBoard[this.Original.At]);

        }
    }
}
