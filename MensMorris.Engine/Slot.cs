using System;
using System.Collections.Generic;
using System.Linq;

namespace MensMorris.Engine
{
    public class Slot
    {
        public event EventHandler IsOnTurnChanged;

        private Match Match;

        public int ID { get; private set; }

        public IPlayer Player { get; private set; }

        private List<Tile> Tiles;

        public bool IsOnTurn { get; private set; }

        internal bool NoPossibleMove { private get; set; }

        public bool HasLost
        {
            get
            {
                return this.NoPossibleMove || this.Tiles.Count(tile => tile.At != null) < 3;
            }
        }

        public bool CanFly
        {
            get
            {
                return this.Tiles.Count(tile => tile.At != null) < 4;
            }
        }

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

        internal PlaceAction DoPlaceAction(Match match)
        {
            // Determine the possible actions
            List<PlaceAction> possibleActions = match.GetEmptyPositions()
                .Select(pos => new PlaceAction(this, this.GetUnusedTile(), pos))
                .ToList();
            // Let the player select one action
            PlaceAction chosenAction = this.Player.ChoosePlaceAction(possibleActions, match);
            if (!possibleActions.Contains(chosenAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            chosenAction.ToPlace.GoTo(chosenAction.Target);
            return chosenAction;
        }

        internal MoveAction DoMoveAction(Match match)
        {
            // Determine the possible actions
            List<MoveAction> possibleActions = this.GetTilesOnBoard()
                .Select(tile => 
                    (this.CanFly ? match.GetEmptyPositions() : tile.At.GetNeighbors().Where(other => other.IsFree))
                    .Select(other => new MoveAction(this, tile, other)))
                .Aggregate((actions1, actions2) => actions1.Concat(actions2))
                .ToList();
            if (possibleActions.Count() == 0)
            {
                return null;
            }
            else
            {
                // Let the player select one action
                MoveAction chosenAction = this.Player.ChooseMoveAction(possibleActions, match);
                if (!possibleActions.Contains(chosenAction)) throw new Exception("Illegal player action!");
                // Execute the selected action
                chosenAction.ToMove.GoTo(chosenAction.Target);
                return chosenAction;
            }
        }

        internal KickAction DoKickAction(Match match)
        {
            IEnumerable<Tile> opponentTiles = match.GetSlot((this.ID == 0) ? 1 : 0).GetTilesOnBoard();
            // Determine the possible actions
            List<KickAction> possibleActions = (opponentTiles.Any(tile => !tile.FormsMill()) ?
                opponentTiles.Where(tile => !tile.FormsMill()) : opponentTiles)
                .Select(tile => new KickAction(this, tile))
                .ToList();
            // Let the player select one action
            KickAction chosenAction = this.Player.ChooseKickAction(possibleActions, match);
            if (!possibleActions.Contains(chosenAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            chosenAction.ToKick.GoTo(null);
            return chosenAction;
        }

        internal void SetOnTurn(bool isOnTurn)
        {
            this.IsOnTurn = isOnTurn;
            this.IsOnTurnChanged?.BeginInvoke(this, EventArgs.Empty, this.IsOnTurnChanged.EndInvoke, null);
        }

        public Slot GetOpponent()
        {
            return this.Match.GetSlot(this.ID == 0 ? 1 : 0);
        }

        public List<Tile> GetTiles()
        {
            // Return copy to prevent manipulation
            return this.Tiles.ToList();
        }

        public List<Tile> GetTilesOnBoard()
        {
            return this.Tiles.Where(tile => tile.At != null).ToList();
        }

        public Tile GetUnusedTile()
        {
            return this.Tiles.First(tile => tile.At == null);
        }

        public bool DoesOwn(BoardPosition pos)
        {
            return (pos != null && pos.Current != null && pos.Current.Owner == this);
        }

    }
}
