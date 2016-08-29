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
        public Tile Tile { get; private set; }

        public BoardPosition Target { get; private set; }

        internal PlaceAction(Slot executing, Tile toPlace, BoardPosition target) : base(executing)
        {
            this.Tile = toPlace;
            this.Target = target;
        }
    }

    public class MoveAction : BaseAction
    {
        public Tile Tile { get; private set; }

        public BoardPosition Target { get; private set; }

        internal MoveAction(Slot executing, Tile toMove, BoardPosition target) : base(executing)
        {
            this.Tile = toMove;
            this.Target = target;
        }
    }

    public class KickAction : BaseAction
    {
        public Tile OpponentTile { get; private set; }

        internal KickAction(Slot executing, Tile toKick) : base(executing)
        {
            this.OpponentTile = toKick;
        }
    }
}
