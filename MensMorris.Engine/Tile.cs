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

        internal void To(BoardPosition target)
        {
            // Reset the previous board position
            if (this.At != null) this.At.Put(null);
            this.At = target;
            // Set the new board position
            if (this.At != null) this.At.Put(this);
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

        #region | Simulation |

        private TileSnapshot Snapshot;

        public bool IsSimulated
        {
            get
            {
                return this.Snapshot != null;
            }
        }

        internal void SimulateTo(BoardPosition simulatedTarget)
        {
            if (this.At != null) this.At.SimulatePut(null);
            if (this.Snapshot == null) this.Snapshot = new TileSnapshot(this.At);
            this.At = simulatedTarget;
            if (this.At != null) this.At.SimulatePut(this);
        }

        internal void Revert()
        {
            if (this.Snapshot != null)
            {
                this.At = this.Snapshot.OriginalAt;
                this.Snapshot = null;
            }
        }

        #endregion // Simulation
    }

    public class TileSnapshot
    {
        public BoardPosition OriginalAt { get; private set; }

        public TileSnapshot(BoardPosition original)
        {
            this.OriginalAt = original;
        }
    }
}
