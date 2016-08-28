using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    public class KickVM
    {
        private BoardVM Parent;
        private BaseAction Action;

        public Point Location { get; set; }

        public int SlotNumber { get; set; }

        public ICommand Choose { get; set; }

        public KickVM(BaseAction action, BoardVM parent, TileVM tile)
        {
            this.Parent = parent;
            this.Action = action;
            this.Location = tile.Location;
            this.SlotNumber = this.Action.Executing.ID;
            this.Choose = new RelayCommand(() => this.Parent.ChooseAction(this.Action));
        }
    }
}
