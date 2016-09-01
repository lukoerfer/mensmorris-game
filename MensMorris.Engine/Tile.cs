using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    /// <summary>
    /// Represents a tile (piece) in a Mens Morris match
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Notifies about a change of the location of the tile
        /// </summary>
        public event EventHandler AtChanged;

        /// <summary>
        /// Gets the owning slot of this tile
        /// </summary>
        public Slot Owner { get; private set; }

        /// <summary>
        /// Gets the board position the slot is located
        /// </summary>
        public BoardPosition At { get; protected set; }

        /// <summary>
        /// Creates a new tile
        /// </summary>
        /// <param name="owner">The slot owning this tile</param>
        public Tile(Slot owner)
        {
            this.Owner = owner;
            this.At = null;
        }

        /// <summary>
        /// Move the tile to a given board position
        /// </summary>
        /// <param name="target">The target board position or null, if the position should be removed from the board</param>
        internal void To(BoardPosition target)
        {
            // Reset the previous board position
            if (this.At != null) this.At.Put(null);
            this.At = target;
            // Set the new board position
            if (this.At != null) this.At.Put(this);
            this.AtChanged?.BeginInvoke(this, EventArgs.Empty, this.AtChanged.EndInvoke, null);
        }

        /// <summary>
        /// Indicates whether this tile forms a mill with two other tiles
        /// </summary>
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

        /// <summary>
        /// Indicates whether this tile is modified by a simulation
        /// </summary>
        public bool IsSimulated
        {
            get
            {
                return this.Snapshot != null;
            }
        }

        /// <summary>
        /// Simulates to move this tile to another board position
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
        /// <param name="simulatedTarget">The target board position</param>
        internal void SimulateTo(BoardPosition simulatedTarget)
        {
            if (this.At != null) this.At.SimulatePut(null);
            if (this.Snapshot == null) this.Snapshot = new TileSnapshot(this.At);
            this.At = simulatedTarget;
            if (this.At != null) this.At.SimulatePut(this);
        }

        /// <summary>
        /// Reverts any changes done to this tile by any simulation
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
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

    /// <summary>
    /// Provides the possibility to save the state of a tile before simulating changes
    /// </summary>
    public class TileSnapshot
    {
        /// <summary>
        /// Gets the position this tile was located on before the simulation
        /// </summary>
        public BoardPosition OriginalAt { get; private set; }

        /// <summary>
        /// Creates a new tile snapshot by storing the original position the tile was located on before the simulation
        /// </summary>
        /// <param name="original">The position the tile was located on before the simulation</param>
        public TileSnapshot(BoardPosition original)
        {
            this.OriginalAt = original;
        }
    }
}
