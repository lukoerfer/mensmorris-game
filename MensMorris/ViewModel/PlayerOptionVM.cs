using MensMorris.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Game.ViewModel
{
    public class PlayerOptionVM
    {
        public Type PlayerType { get; set; }

        private String Name;

        public PlayerOptionVM(Type playerType)
        {
            this.PlayerType = playerType;
            this.Name = this.PlayerType != null ? (Activator.CreateInstance(this.PlayerType) as IPlayer).GetName() : "Player";
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
