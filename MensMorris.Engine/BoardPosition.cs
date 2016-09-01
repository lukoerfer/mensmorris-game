using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    /// <summary>
    /// Represents a position on the board of a Mens Morris match
    /// </summary>
    public class BoardPosition
    {
        /// <summary>
        /// Gets the ring in which the position is located
        /// </summary>
        public int Ring { get; private set; }
        /// <summary>
        /// Gets the number of this position in its ring
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Gets the parent match of this position
        /// </summary>
        public Match Match { get; private set; }

        // Each neighbor position is accessed by its direction from this position
        protected Dictionary<Direction, BoardPosition> Neighbors;

        /// <summary>
        /// Gets the tile currently located on this position
        /// </summary>
        public Tile Current { get; protected set; }

        /// <summary>
        /// Indicates whether this position has no tile located on it
        /// </summary>
        public bool IsFree
        {
            get
            {
                return this.Current == null;
            }
        }

        /// <summary>
        /// Creates a new board position
        /// </summary>
        /// <param name="match">The parent match</param>
        /// <param name="ring">The ring where the position is located</param>
        /// <param name="number">The number in the mentioned ring</param>
        public BoardPosition(Match match, int ring, int number)
        {
            this.Match = match;
            this.Ring = ring;
            this.Number = number;
            this.Neighbors = new Dictionary<Direction, BoardPosition>();
            this.Current = null;
        }

        /// <summary>
        /// Sets a neighbor of this position
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
        /// <param name="direction">The direction where the neighbor position is located</param>
        /// <param name="position">The neighbor position</param>
        internal void SetNeighbor(Direction direction, BoardPosition position)
        {
            this.Neighbors.Add(direction, position);
        }

        /// <summary>
        /// Puts a tile onto this position
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
        /// <param name="tile">The tile</param>
        internal void Put(Tile tile)
        {
            this.Current = tile;
        }

        /// <summary>
        /// Gets the neighbor position located in a given direction
        /// </summary>
        /// <param name="direction">The requested direction</param>
        /// <returns>The found neighbor position or null, if there is no neighbor in this direction</returns>
        public BoardPosition Neighbor(Direction direction)
        {
            BoardPosition result;
            return this.Neighbors.TryGetValue(direction, out result) ? result : null;
        }

        /// <summary>
        /// Get all neighbor positions
        /// </summary>
        public List<BoardPosition> GetNeighbors()
        {
            return this.Neighbors.Values.ToList();
        }

        #region | Simulation |

        private BoardPositionSnapshot Snapshot;

        /// <summary>
        /// Indicates whether this position is modified by a simulation
        /// </summary>
        public bool IsSimulated
        {
            get
            {
                return this.Snapshot != null;
            }
        }

        /// <summary>
        /// Simulates to put a tile onto this position
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
        /// <param name="simulatedTile">The tile</param>
        internal void SimulatePut(Tile simulatedTile)
        {
            if (this.Snapshot == null) this.Snapshot = new BoardPositionSnapshot(this.Current);
            this.Current = simulatedTile;
        }

        /// <summary>
        /// Reverts any changes done to this position by any simulation
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
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

    /// <summary>
    /// Provides the possibility to save the state of a board position before simulating changes
    /// </summary>
    public class BoardPositionSnapshot
    {
        /// <summary>
        /// Gets the tile located on the position before the simulation
        /// </summary>
        public Tile OriginalCurrent { get; private set; }

        /// <summary>
        /// Creates a new position snapshot by storing the original tile located on the position before the simulation
        /// </summary>
        /// <param name="original">The tile located on the position before the simulation</param>
        public BoardPositionSnapshot(Tile original)
        {
            this.OriginalCurrent = original;
        }
    }

}
