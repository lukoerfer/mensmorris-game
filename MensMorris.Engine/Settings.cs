using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class Settings
    {
        public int RingCount { get; set; }

        public bool CenterPoint { get; set; }

        public bool CenterCross { get; set; }

        public bool ConnectCorners { get; set; }

        public int TilesPerSlot { get; set; }

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
