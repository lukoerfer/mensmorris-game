using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class PlaceAction
    {
        public Tile PlacedTile { get; private set; }

        public BoardPosition TargetPosition { get; private set; }

        public PlaceAction(Tile placed, BoardPosition target)
        {
            this.PlacedTile = placed;
            this.TargetPosition = target;
        }

    }
}
