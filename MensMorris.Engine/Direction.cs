using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    /// <summary>
    /// Enumerates the possible connection directions on a Mens Morris board
    /// </summary>
    /// <remarks>Between two succeeding directions is an angle of 45°</remarks>
    public enum Direction
    {
        Down = 0,
        DownRight = 1,
        Right = 2,
        UpRight = 3,
        Up = 4,
        UpLeft = 5,
        Left = 6,
        DownLeft = 7
    }

    /// <summary>
    /// Provides a set of static helper functions for working with directions
    /// </summary>
    public static class DirectionHelpers
    {
        /// <summary>
        /// Returns a list of all possible board directions
        /// </summary>
        public static List<Direction> Enumerate()
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        }

        /// <summary>
        /// Determines the main direction for a given direction
        /// </summary>
        /// <remarks>
        /// This basically removes the orientation from the direction vector.
        /// E.g., Top and Down have the same direction, but another orientation.
        /// By applying this function, they get mapped to the same direction.
        /// </remarks>
        public static Direction ToMain(this Direction direction)
        {
            int directionValue = (int)direction;
            if (directionValue >= 4)
            {
                // Inverse the second half of the directions
                return (Direction)(directionValue - 4);
            }
            else
            {
                return direction;
            }
        }

        /// <summary>
        /// Determines the onwards direction for a given board ring side
        /// </summary>
        /// <param name="sideNumber">The number of the ring side</param>
        /// <remarks>
        /// E.g., side number 0 means the left side:
        /// On this side the board positions are connected in Down direction.
        /// </remarks>
        public static Direction ForSide(int sideNumber)
        {
            return (Direction)(sideNumber * 2);
        }

        /// <summary>
        /// Turns a direction x times 45° counter-clockwise
        /// </summary>
        /// <param name="direction">The base direction</param>
        /// <param name="times45">The amount of times to turn around 45°</param>
        /// <returns>The new calculated direction</returns>
        public static Direction Turn(this Direction direction, int times45)
        {
            return (Direction)(((int)direction + times45) % 8);
        }

        /// <summary>
        /// Determines the opposite direction for a given direction
        /// </summary>
        /// <param name="direction">The base direction</param>
        /// <returns>The calculated opposite direction</returns>
        public static Direction Opposite(this Direction direction)
        {
            return (Direction)(((int)direction + 4) % 8);
        }
    }
}
