using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
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

    public static class DirectionHelpers
    {
        public static IEnumerable<Direction> Enumerate()
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>();
        }

        public static Direction ForSide(int sideNumber)
        {
            return (Direction)(sideNumber * 2);
        }

        public static Direction Turn(this Direction direction, int times45)
        {
            return (Direction)(((int)direction + times45) % 8);
        }

        public static Direction Opposite(this Direction direction)
        {
            return (Direction)(((int)direction + 4) % 8);
        }
    }
}
