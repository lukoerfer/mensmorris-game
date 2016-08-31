using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    /// <summary>
    /// Represents a player slot in a Mens Morris match
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// Notifies when it started or stopped to be this slots turn
        /// </summary>
        public event EventHandler<bool> IsOnTurnChanged;

        /// <summary>
        /// Gets the match this slot participates in
        /// </summary>
        public Match Match { get; private set; }

        /// <summary>
        /// Gets the id of this slot
        /// </summary>
        /// <remarks>
        /// This is not used for any access inside the engine.
        /// Games can use this ID to assign e.g. colors to the different slots.
        /// </remarks>
        public int ID { get; private set; }

        /// <summary>
        /// Gets the player playing in this match slot
        /// </summary>
        public IPlayer Player { get; private set; }

        // Stores each tile assigned to this slot
        private List<Tile> Tiles;

        /// <summary>
        /// Indicates whether its this slots turn
        /// </summary>
        public bool IsOnTurn { get; private set; }

        /// <summary>
        /// Sets whether this slot had no possible move on one of his turns
        /// </summary>
        /// <remarks>
        /// This can only be done from inside the game engine
        /// </remarks>
        internal bool NoPossibleMove { private get; set; }

        /// <summary>
        /// Indicates whether this slot lost the match
        /// </summary>
        public bool HasLost
        {
            get
            {
                return this.NoPossibleMove || this.Tiles.Count(tile => tile.At != null) < 3;
            }
        }

        /// <summary>
        /// Indicates whether this slot is allowed to "fly" his tiles in his moves
        /// </summary>
        public bool CanFly
        {
            get
            {
                return this.Tiles.Count(tile => tile.At != null) < 4;
            }
        }

        /// <summary>
        /// Creates a new player slot
        /// </summary>
        /// <param name="match">The match the slot participates in</param>
        /// <param name="player">The player playing in the slot</param>
        /// <param name="id">The number of the slot</param>
        public Slot(Match match, IPlayer player, int id)
        {
            this.Match = match;
            this.ID = id;
            this.Player = player;
            this.Tiles = new List<Tile>();
            for (int i = 0; i < this.Match.Settings.TilesPerSlot; i++)
            {
                this.Tiles.Add(new Tile(this));
            }
        }

        /// <summary>
        /// Lets the slot determine all possible placement actions and its player choose one
        /// </summary>
        /// <returns>The chosen possible action or null, if no such action is available or the player returned an invalid action</returns>
        internal PlaceAction HandlePlaceAction()
        {
            // Determine the possible actions
            List<PlaceAction> possibleActions = this.Match.GetEmptyPositions()
                .Select(pos => new PlaceAction(this, this.GetUnusedTile(), pos))
                .ToList();
            // Let the player select one action
            PlaceAction chosenAction = this.Player.ChoosePlaceAction(possibleActions, this.Match);
            // Return chosen action if it is possible
            return possibleActions.Contains(chosenAction) ? chosenAction : null;
        }

        /// <summary>
        /// Lets the slot determine all possible movement actions and its player choose one
        /// </summary>
        /// <returns>The chosen possible action or null, if no such action is available or the player returned an invalid action</returns>
        internal MoveAction HandleMoveAction()
        {
            // Determine the possible actions
            List<MoveAction> possibleActions = this.GetTilesOnBoard()
                .Select(tile => 
                    (this.CanFly ? this.Match.GetEmptyPositions() : tile.At.GetNeighbors().Where(other => other.IsFree))
                    .Select(other => new MoveAction(this, tile, other)))
                .Aggregate((actions1, actions2) => actions1.Concat(actions2))
                .ToList();
            if (possibleActions.Count() == 0)
            {
                return null;
            }
            else
            {
                // Let the player choose one action
                MoveAction chosenAction = this.Player.ChooseMoveAction(possibleActions, this.Match);
                // Return chosen action if it is possible
                return possibleActions.Contains(chosenAction) ? chosenAction : null;
            }
        }

        /// <summary>
        /// Lets the slot determine all possible kick actions and its player choose one
        /// </summary>
        /// <returns>The chosen possible action or null, if no such action is available or the player returned an invalid action</returns>
        internal KickAction HandleKickAction()
        {
            // Determine the tiles of the opponent slot on the board
            IEnumerable<Tile> opponentTiles = this.Match.GetSlot((this.ID == 0) ? 1 : 0).GetTilesOnBoard();
            // Determine the possible kick actions
            List<KickAction> possibleActions = (
                // Tiles from mills are only allowed if there are no other tiles
                opponentTiles.Any(tile => !tile.FormsMill()) ?
                opponentTiles.Where(tile => !tile.FormsMill()) : opponentTiles)
                .Select(tile => new KickAction(this, tile))
                .ToList();
            // Let the player select one action
            KickAction chosenAction = this.Player.ChooseKickAction(possibleActions, this.Match);
            // Return action if it is possible
            return possibleActions.Contains(chosenAction) ? chosenAction : null;
        }

        /// <summary>
        /// Sets whether its this slots turn
        /// </summary>
        internal void SetOnTurn(bool isOnTurn)
        {
            this.IsOnTurn = isOnTurn;
            this.IsOnTurnChanged?.BeginInvoke(this, this.IsOnTurn, this.IsOnTurnChanged.EndInvoke, null);
        }

        /// <summary>
        /// Determines this slots opponent slot
        /// </summary>
        /// <returns>The other slot in this slots match</returns>
        public Slot GetOpponent()
        {
            return this.Match.GetSlot(this.ID == 0 ? 1 : 0);
        }

        /// <summary>
        /// Gets a list of all tiles assigned to this slot
        /// </summary>
        public List<Tile> GetTiles()
        {
            // Return copy to prevent manipulation
            return this.Tiles.ToList();
        }

        /// <summary>
        /// Gets a list of all tiles assigned to this slot which are located on the board
        /// </summary>
        public List<Tile> GetTilesOnBoard()
        {
            return this.Tiles.Where(tile => tile.At != null).ToList();
        }

        /// <summary>
        /// Finds a tile assigned to this slot which is not located on the board
        /// </summary>
        public Tile GetUnusedTile()
        {
            return this.Tiles.First(tile => tile.At == null);
        }

        /// <summary>
        /// Indicates whether a given position has tile assigned to this slot located on it
        /// </summary>
        /// <param name="pos">The questionable board position</param>
        public bool DoesOwn(BoardPosition pos)
        {
            return (pos != null && pos.Current != null && pos.Current.Owner == this);
        }
    }
}
