namespace MensMorris.Engine
{
    public abstract class BaseAction
    {
        public Slot Executing { get; private set; }

        public BaseAction(Slot executing)
        {
            this.Executing = executing;
        }
    }

    public class PlaceAction : BaseAction
    {
        public Tile ToPlace { get; private set; }

        public BoardPosition Target { get; private set; }

        internal PlaceAction(Slot executing, Tile toPlace, BoardPosition target) : base(executing)
        {
            this.ToPlace = toPlace;
            this.Target = target;
        }
    }

    public class MoveAction : BaseAction
    {
        public Tile ToMove { get; private set; }

        public BoardPosition Target { get; private set; }

        internal MoveAction(Slot executing, Tile toMove, BoardPosition target) : base(executing)
        {
            this.ToMove = toMove;
            this.Target = target;
        }
    }

    public class KickAction : BaseAction
    {
        public Tile ToKick { get; private set; }

        internal KickAction(Slot executing, Tile toKick) : base(executing)
        {
            this.ToKick = toKick;
        }
    }
}
