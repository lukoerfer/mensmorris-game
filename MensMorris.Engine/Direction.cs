using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public enum Direction
    {
        // Lower in number direction
        Down = 0,
        Y_Minus = 0,
        LowerNumber = 0,
        // Higher in letter direction
        Right = 1,
        X_Plus = 1,
        HigherLetter = 1,
        // Higher in number direction
        Up = 2,
        Y_Plus = 2,
        HigherNumber = 2,
        // Lower in letter direction
        Left = 3,
        X_Minus = 3,
        LowerLetter = 3
    }

    public static class DirectionHelpers
    {
        public static IEnumerable<Direction> Enumerate()
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>();
        }

        public static Direction Next(this Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public static Direction Opposite(this Direction direction)
        {
            return (Direction)(((int)direction + 2) % 4);
        }
    }
}
