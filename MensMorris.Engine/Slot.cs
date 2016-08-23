using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public class Slot
    {
        public event EventHandler IsOnTurnChanged;

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

        public Slot(IPlayer player, int id)
        {
            this.ID = id;
            this.Player = player;
            this.Tiles = new List<Tile>();
            for (int i = 0; i < 9; i++)
            {
                this.Tiles.Add(new Tile(this));
            }
        }

        internal PlaceAction DoPlaceAction(Match match)
        {
            // Determine the possible actions
            List<PlaceAction> possibleActions = match.GetEmptyPositions()
                .Select(pos => new PlaceAction(this.GetUnusedTile(), pos))
                .ToList();
            // Let the player select one action
            PlaceAction selectedAction = this.Player.SelectPlaceAction(possibleActions, match);
            if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            selectedAction.ToPlace.GoTo(selectedAction.Target);
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
                MoveAction selectedAction = this.Player.SelectMoveAction(possibleActions, match);
                if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action!");
                // Execute the selected action
                selectedAction.ToMove.GoTo(selectedAction.Target);
                return selectedAction;
            }
        }

        internal KickAction DoKickAction(Match match)
        {
            IEnumerable<Tile> opponentTiles = match.GetSlot((this.ID == 0) ? 1 : 0).GetTilesOnBoard();
            // Determine the possible actions
            List<KickAction> possibleActions = (opponentTiles.Any(tile => !tile.FormsMill()) ?
                opponentTiles.Where(tile => !tile.FormsMill()) : opponentTiles)
                .Select(tile => new KickAction(tile))
                .ToList();
            // Let the player select one action
            KickAction selectedAction = this.Player.SelectKickAction(possibleActions, match);
            if (!possibleActions.Contains(selectedAction)) throw new Exception("Illegal player action");
            // Execute the selected action
            selectedAction.ToKick.GoTo(null);
            return selectedAction;
        }

        internal void SetIsOnTurn(bool isOnTurn)
        {
            this.IsOnTurn = isOnTurn;
            this.IsOnTurnChanged?.BeginInvoke(this, EventArgs.Empty, this.IsOnTurnChanged.EndInvoke, null);
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
