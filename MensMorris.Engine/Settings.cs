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

        public bool HasCenter { get; set; }

        public bool ConnectDiagonals { get; set; }

        public int TilesPerSlot { get; set; }

        public Settings(int rings, bool hasCenter, bool diagonals, int tiles)
        {
            this.RingCount = rings;
            this.HasCenter = hasCenter;
            this.ConnectDiagonals = diagonals;
            this.TilesPerSlot = tiles;
        }

    }
}
