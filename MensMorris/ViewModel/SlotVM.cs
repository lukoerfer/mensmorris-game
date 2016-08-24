using System;

using MensMorris.Engine;
using PropertyChanged;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class SlotVM
    {
        private Slot Model;

        [DoNotNotify]
        public int SlotNumber { get; set; }

        [DoNotNotify]
        public string Name { get; set; }

        public bool IsOnTurn { get; set; }

        public SlotVM(Slot slot)
        {
            this.Model = slot;
            this.Model.IsOnTurnChanged += OnIsOnTurnChanged;
            this.SlotNumber = this.Model.ID;
            this.Name = this.Model.Player.GetName();
            this.IsOnTurn = this.Model.IsOnTurn;
        }

        private void OnIsOnTurnChanged(object sender, EventArgs e)
        {
            this.IsOnTurn = this.Model.IsOnTurn;
        }
    }
}
