using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public abstract class BaseAction
    {
        public Slot Executing { get; private set; }
    }

    public class PlaceAction : BaseAction
    {
        public Tile ToPlace { get; private set; }

        public BoardPosition Target { get; private set; }

        internal PlaceAction(Tile toPlace, BoardPosition target)
        {
            this.ToPlace = toPlace;
            this.Target = target;
        }
    }

    public class MoveAction : BaseAction
    {
        public Tile ToMove { get; private set; }

        public BoardPosition Target { get; private set; }

        internal MoveAction(Tile toMove, BoardPosition target)
        {
            this.ToMove = toMove;
            this.Target = target;
        }
    }

    public class KickAction : BaseAction
    {
        public Tile ToKick { get; private set; }

        internal KickAction(Tile toKick)
        {
            this.ToKick = toKick;
        }
    }
}
