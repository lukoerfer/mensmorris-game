using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class Slot
    {
        public int ID { get; private set; }

        private IPlayer player;

        private List<Tile> tiles;

        public IEnumerable<Tile> Tiles
        {
            get
            {
                // Return copy to prevent manipulation
                return this.tiles.ToList();
            }
        }

        internal bool NoPossibleMove { private get; set; }

        public bool HasLost
        {
            get
            {
                return this.NoPossibleMove || this.tiles.Count(tile => tile.At != null) < 3;
            }
        }

        public Slot(IPlayer player, int id)
        {
            this.ID = id;
            this.player = player;
            this.tiles = new List<Tile>();
            for (int i = 0; i < 9; i++)
            {
                this.tiles.Add(new Tile(this));
            }
        }

        internal PlaceAction DoPlaceAction(Match match)
        {
            // Determine the possible actions
            List<PlaceAction> possibleActions = match.GetEmptyPositions()
                .Select(pos => new PlaceAction(this.GetUnusedTile(), pos))
                .ToList();
            // Let the player select one action
            PlaceAction selectedAction = this.player.SelectPlaceAction(possibleActions, match);
            if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            selectedAction.PlacedTile.GoTo(selectedAction.TargetPosition);
            return selectedAction;
        }

        internal MoveAction DoMoveAction(Match match)
        {
            // Determine the possible actions
            List<MoveAction> possibleActions = this.GetTilesOnBoard()
                .Select(tile => tile.At.GetNeighbors()
                    .Where(neigh => neigh.IsFree)
                    .Select(neigh => new MoveAction(tile, neigh)))
                .Aggregate((actions1, actions2) => actions1.Concat(actions2))
                .ToList();
            if (possibleActions.Count() == 0)
            {
                return null;
            }
            else
            {
                // Let the player select one action
                MoveAction selectedAction = this.player.SelectMoveAction(possibleActions, match);
                if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action!");
                // Execute the selected action
                selectedAction.MovingTile.GoTo(selectedAction.TargetPosition);
                return selectedAction;
            }
        }

        internal KickAction DoKickAction(Match match)
        {
            // Determine the possible actions
            List<KickAction> possibleActions = match.GetSlot((this.ID == 0) ? 1 : 0)
                .GetTilesOnBoard()
                .Select(tile => new KickAction(tile))
                .ToList();
            // Let the player select one action
            KickAction selectedAction = this.player.SelectKickAction(possibleActions, match);
            if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            selectedAction.KickedTile.GoTo(null);
            return selectedAction;
        }

        public IEnumerable<Tile> GetTilesOnBoard()
        {
            return this.tiles.Where(tile => tile.At != null);
        }

        public Tile GetUnusedTile()
        {
            return this.tiles.First(tile => tile.At == null);
        }

        public bool DoesOwn(BoardPosition pos)
        {
            return (pos != null && pos.Current != null && pos.Current.Owner == this);
        }

    }
}
