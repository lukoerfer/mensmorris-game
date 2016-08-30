using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class BoardVM
    {
        public event Action<BaseAction> ActionChosen;

        public List<PositionVM> Positions { get; private set; }

        public List<ConnectionVM> Connections { get; private set; }

        // Use an observable collection, because tiles can change their position
        public ObservableCollection<TileVM> Tiles { get; private set; }

        public List<TargetVM> Targets { get; private set; }

        public List<KickVM> Kicks { get; private set; }

        public ObservableCollection<object> BoardElements
        {
            get
            {
                return new ObservableCollection<object>(
                    this.Connections.Cast<object>()
                    .Concat(this.Positions.Cast<object>())
                    .Concat(this.Tiles.Cast<object>())
                    .Concat(this.Targets.Cast<object>())
                    .Concat(this.Kicks.Cast<object>())
                );
            }
        }

        public BoardVM(List<BoardPosition> board, List<Tile> tiles)
        {
            // Create a view model for each board position
            this.Positions = board.Select(pos => new PositionVM(this, pos)).ToList();
            // Create a view model for each connection between two board positions
            this.Connections = board
                .Select(
                    pos => pos.GetNeighbors()
                        .Where(neigh => pos.Ring < neigh.Ring || ( pos.Ring == neigh.Ring && pos.Number < neigh.Number))
                        .Select(neigh => new ConnectionVM(this, this.GetPosition(pos), this.GetPosition(neigh))
                    ))
                .Aggregate((one, two) => one.Union(two))
                .ToList();
            // Create a view model for each tile
            this.Tiles = new ObservableCollection<TileVM>(
                tiles.Select(tile => new TileVM(this, tile))
            );
            // Empty list for targets
            this.Targets = new List<TargetVM>();
            this.Kicks = new List<KickVM>();
        }

        public PositionVM GetPosition(BoardPosition model)
        {
            return this.Positions.Single(pos => pos.Model.Equals(model));
        }

        public TileVM GetTile(Tile model)
        {
            return this.Tiles.Single(tile => tile.Model.Equals(model));
        }

        public void SetSelectedTile(List<TargetVM> relatedTargets)
        {
            // Reset the current selected tile
            this.Tiles.ToList().ForEach(tile => tile.IsSelected = false);
            // Set the targets for the selected tile
            this.Targets = relatedTargets;
        }

        public void SetPlaceActions(List<PlaceAction> actions)
        {
            this.Targets = actions.Select(action => new TargetVM(action, this, this.GetPosition(action.Target))).ToList();
        }

        public void SetMoveActions(List<MoveAction> actions)
        {
            actions.ForEach(action => this.GetTile(action.Tile).RelatedTargets.Add(new TargetVM(action, this, this.GetPosition(action.Target))));
        }

        public void SetKickActions(List<KickAction> actions)
        {
            this.Kicks = actions.Select(action => new KickVM(action, this, this.GetTile(action.OpponentTile))).ToList();
        }

        public void ChooseAction(BaseAction action)
        {
            // Reset related targets and selection state for each tile
            foreach (TileVM tile in this.Tiles)
            {
                tile.RelatedTargets.Clear();
                tile.IsSelected = false;
            }
            // Reset the targets
            this.Targets = new List<TargetVM>();
            // Reset the kick options
            this.Kicks = new List<KickVM>();
            this.ActionChosen?.Invoke(this, action);
        }

    }
}
