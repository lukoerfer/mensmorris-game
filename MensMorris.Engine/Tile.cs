using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class Tile
    {
        public event EventHandler AtChanged;

        public Slot Owner { get; private set; }

        public BoardPosition At { get; private set; }

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
}
