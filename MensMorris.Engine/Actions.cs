namespace MensMorris.Engine
{
    /// <summary>
    /// Base class for Mens Morris actions
    /// </summary>
    public abstract class BaseAction
    {
        /// <summary>
        /// Gets the slot executing this action
        /// </summary>
        public Slot Executing { get; private set; }

        /// <summary>
        /// Base constructor for Mens Morris actions
        /// </summary>
        /// <param name="executing">The slot executing the action</param>
        public BaseAction(Slot executing)
        {
            this.Executing = executing;
        }
    }

    /// <summary>
    /// 
    /// </summary>
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
