using MensMorris.Engine;
using System;

namespace MensMorris.Game.ViewModel
{
    public class PlayerOptionVM
    {
        public Type PlayerType { get; set; }

        private string Name;

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
