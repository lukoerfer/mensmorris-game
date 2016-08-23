using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class BoardVM
    {

        public IEnumerable<PositionVM> Positions { get; private set; }

        public IEnumerable<ConnectionVM> Connections { get; private set; }

        public ObservableCollection<TileVM> Tiles { get; private set; }

        public ObservableCollection<object> BoardElements
        {
            get
            {
                return new ObservableCollection<object>(
                    this.Connections.Cast<object>()
                    .Concat(this.Positions.Cast<object>())
                    .Concat(this.Tiles.Cast<object>())
                );
            }
        }

        public BoardVM(List<BoardPosition> board, List<Tile> tiles)
        {
            // Create a view model for each board position
            this.Positions = board.Select(pos => new PositionVM(this, pos));
            // Create a view model for each connection between two board positions
            this.Connections = board
                .Select(
                    pos => pos.GetNeighbors()
                        .Where(neigh => pos.Ring < neigh.Ring || ( pos.Ring == neigh.Ring && pos.Number < neigh.Number))
                        .Select(neigh => new ConnectionVM(this, GetPosition(pos.Ring, pos.Number), GetPosition(neigh.Ring, neigh.Number))
                    ))
                .Aggregate((one, two) => one.Union(two));
            // Create a view model for each tile
            this.Tiles = new ObservableCollection<TileVM>(
                tiles.Select(tile => new TileVM(this, tile))
            );
        }

        public PositionVM GetPosition(int ring, int number)
        {
            return this.Positions.Single(pos => pos.Model.Ring == ring && pos.Model.Number == number);
        }
    }
}
