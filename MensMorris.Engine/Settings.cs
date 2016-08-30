using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    /// <summary>
    /// Represents a set of Mens Morris settings (board configuration)
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets the number of rings to create on the board
        /// </summary>
        public int RingCount { get; private set; }

        /// <summary>
        /// Gets whether to create a point in the center of the board
        /// </summary>
        public bool CenterPoint { get; private set; }

        /// <summary>
        /// Gets whether to connect the opposing middle positions of the inner ring
        /// </summary>
        public bool CenterCross { get; private set; }

        /// <summary>
        /// Gets whether to connect the corner points of succeeding rings
        /// </summary>
        public bool ConnectCorners { get; private set; }

        /// <summary>
        /// Gets the number of tiles per player slot
        /// </summary>
        public int TilesPerSlot { get; private set; }

        /// <summary>
        /// Creates a new set of Mens Morris settings
        /// </summary>
        /// <param name="rings">The number of rings to create on the board</param>
        /// <param name="centerPoint">Whether to create a point in the center of the board</param>
        /// <param name="centerCross">Whether to connect the opposing middle positions of the inner ring</param>
        /// <param name="diagonals">Whether to connect the corner points of succeeding rings</param>
        /// <param name="tiles">The number of tiles per player slot</param>
        public Settings(int rings, bool centerPoint, bool centerCross, bool diagonals, int tiles)
        {
            this.RingCount = rings;
            this.CenterPoint = centerPoint;
            this.CenterCross = centerCross;
            this.ConnectCorners = diagonals;
            this.TilesPerSlot = tiles;
        }
    }
}
