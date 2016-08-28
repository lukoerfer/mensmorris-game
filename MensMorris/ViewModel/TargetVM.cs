using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    public class TargetVM
    {
        private BoardVM Parent;
        private BaseAction Action;

        public Point Location { get; set; }

        public int SlotNumber { get; set; }

        public ICommand Choose { get; set; }

        public TargetVM(BaseAction action, BoardVM parent, PositionVM position)
        {
            this.Action = action;
            this.Parent = parent;
            this.Location = position.Location;
            this.SlotNumber = this.Action.Executing.ID;
            this.Choose = new RelayCommand(() => this.Parent.ChooseAction(this.Action));
        }
    }
}
