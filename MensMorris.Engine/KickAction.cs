using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class KickAction
    {
        public Tile KickedTile { get; private set; }

        public KickAction(Tile kicked)
        {
            this.KickedTile = kicked;
        }
    }
}
