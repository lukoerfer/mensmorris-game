using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for the Mens Morris board
    /// </summary>
    [ImplementPropertyChanged]
    public class BoardVM
    {
        public event Action<BaseAction> ActionChosen;

        /// <summary>
        /// Stores the view models for the positions on the board
        /// </summary>
        public List<PositionVM> Positions { get; private set; }

        /// <summary>
        /// Stores the view models for the connections between the positions on the board
        /// </summary>
        public List<ConnectionVM> Connections { get; private set; }

        /// <summary>
        /// Stores the view models for the tiles on the board
        /// </summary>
        /// <remarks>
        /// An observable collection is used, because tiles can change their position.
        /// </remarks>
        public ObservableCollection<TileVM> Tiles { get; private set; }

        /// <summary>
        /// Stores the view models for the placement / movement targets
        /// </summary>
        public List<TargetVM> Targets { get; private set; }

        /// <summary>
        /// Stores the view models for the kick actions
        /// </summary>
        public List<KickVM> Kicks { get; private set; }

        /// <summary>
        /// Offers all required view models for binding of the board
        /// </summary>
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

        /// <summary>
        /// Creates a new board view model
        /// </summary>
        /// <param name="board">All board positions</param>
        /// <param name="tiles">All tiles in the match</param>
        public BoardVM(List<BoardPosition> board, List<Tile> tiles)
        {
            // Create a view model for each board position
            this.Positions = board.Select(pos => new PositionVM(this, pos)).ToList();
            // Create a view model for each connection between two board positions
            this.Connections = board
                .Select(
                    pos => pos.GetNeighbors()
                        .Where(neigh => pos.Ring < neigh.Ring || ( pos.Ring == neigh.Ring && pos.Number < neigh.Number))
                        .Select(neigh => new ConnectionVM(this.GetPosition(pos), this.GetPosition(neigh))
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

        /// <summary>
        /// Find a position view model assigned to a specific position model
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>The assigned view model</returns>
        public PositionVM GetPosition(BoardPosition model)
        {
            return this.Positions.Single(pos => pos.Model.Equals(model));
        }

        /// <summary>
        /// Find a tile view model assigned to specific tile model
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>The assigned view model</returns>
        public TileVM GetTile(Tile model)
        {
            return this.Tiles.Single(tile => tile.Model.Equals(model));
        }

        /// <summary>
        /// Resets the selection of all other tiles and offers a list of targets to the user related to the new selected tile
        /// </summary>
        /// <param name="relatedTargets">The list of target related to the new selected tile</param>
        public void SetSelectedTile(List<TargetVM> relatedTargets)
        {
            // Reset the current selected tile
            this.Tiles.ToList().ForEach(tile => tile.IsSelected = false);
            // Set the targets for the selected tile
            this.Targets = relatedTargets;
        }

        /// <summary>
        /// Makes the possible placement actions available to the user
        /// </summary>
        /// <param name="actions">The possible placement actions</param>
        public void SetPlaceActions(List<PlaceAction> actions)
        {
            this.Targets = actions.Select(action => new TargetVM(action, this, this.GetPosition(action.Target))).ToList();
        }

        /// <summary>
        /// Makes the possible movement actions available to the user
        /// </summary>
        /// <param name="actions">The possible movement actions</param>
        public void SetMoveActions(List<MoveAction> actions)
        {
            actions.ForEach(action => this.GetTile(action.Tile).RelatedTargets.Add(new TargetVM(action, this, this.GetPosition(action.Target))));
        }

        /// <summary>
        /// Makes the possible kick actions available to the user
        /// </summary>
        /// <param name="actions">The possible kick actions</param>
        public void SetKickActions(List<KickAction> actions)
        {
            this.Kicks = actions.Select(action => new KickVM(action, this, this.GetTile(action.OpponentTile))).ToList();
        }

        /// <summary>
        /// Routes a chosen action and resets the action possibilies
        /// </summary>
        /// <param name="action">The chosen action</param>
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
            // Notify about the chosen action
            this.ActionChosen?.Invoke(action);
        }

    }
}
