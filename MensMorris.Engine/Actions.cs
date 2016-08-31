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
    /// Defines a Mens Morris tile placement
    /// </summary>
    public class PlaceAction : BaseAction
    {
        /// <summary>
        /// Gets the tile to place onto the board
        /// </summary>
        public Tile Tile { get; private set; }

        /// <summary>
        /// Gets the placement target position
        /// </summary>
        public BoardPosition Target { get; private set; }

        /// <summary>
        /// Creates a new tile placement action
        /// </summary>
        /// <param name="executing">The slot executing the action</param>
        /// <param name="tile">The tile to place</param>
        /// <param name="target">The target of the tile placement</param>
        internal PlaceAction(Slot executing, Tile tile, BoardPosition target) : base(executing)
        {
            this.Tile = tile;
            this.Target = target;
        }
    }

    /// <summary>
    /// Defines a Mens Morris tile movement
    /// </summary>
    public class MoveAction : BaseAction
    {
        /// <summary>
        /// Gets the tile to move on the board
        /// </summary>
        public Tile Tile { get; private set; }

        /// <summary>
        /// Gets the moves target position
        /// </summary>
        public BoardPosition Target { get; private set; }

        /// <summary>
        /// Creates a new tile movement action
        /// </summary>
        /// <param name="executing">The slot executing the action</param>
        /// <param name="tile">The tile to move</param>
        /// <param name="target">The target of the tile movement</param>
        internal MoveAction(Slot executing, Tile tile, BoardPosition target) : base(executing)
        {
            this.Tile = tile;
            this.Target = target;
        }
    }

    /// <summary>
    /// Define a Mens Morris tile kick
    /// </summary>
    public class KickAction : BaseAction
    {
        /// <summary>
        /// Gets the opponent tile to kick from the board
        /// </summary>
        public Tile OpponentTile { get; private set; }

        /// <summary>
        /// Creates a new tile kick action
        /// </summary>
        /// <param name="executing">The slot executing the action</param>
        /// <param name="opponentTile">The opponent tile to kick</param>
        internal KickAction(Slot executing, Tile opponentTile) : base(executing)
        {
            this.OpponentTile = opponentTile;
        }
    }
}
