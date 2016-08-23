using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MensMorris.Engine;
using PropertyChanged;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class SlotVM
    {
        private Slot Model;

        public String Name { get; set; }

        public bool IsOnTurn { get; set; }

        public SlotVM(Slot slot)
        {
            this.Model = slot;
            this.Model.IsOnTurnChanged += OnIsOnTurnChanged;
            this.Name = this.Model.Player.GetName();
            this.IsOnTurn = this.Model.IsOnTurn;
        }

        private void OnIsOnTurnChanged(object sender, EventArgs e)
        {
            this.IsOnTurn = this.Model.IsOnTurn;
        }
    }
}
