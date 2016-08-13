using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class MoveAction
    {
        public Tile MovingTile { get; private set; }

        public BoardPosition TargetPosition { get; private set; }

        public MoveAction(Tile moving, BoardPosition target)
        {
            this.MovingTile = moving;
            this.TargetPosition = target;
        }
    }
}
