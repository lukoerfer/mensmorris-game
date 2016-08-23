using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class StartVM
    {
        public event EventHandler StartDone;

        public List<PlayerOptionVM> PlayerOptions { get; set; }

        public PlayerOptionVM FirstPlayer { get; set; }
        public PlayerOptionVM SecondPlayer { get; set; }

        public ICommand StartMatch { get; set; }

        public StartVM()
        {
            this.PlayerOptions = new List<PlayerOptionVM>();
            // Add human player option
            this.PlayerOptions.Add(new PlayerOptionVM(null));
            // Add all available bots to the player options
            this.PlayerOptions.AddRange(BotCollector.CollectBots().Select(botType => new PlayerOptionVM(botType)));
            // Init start match button
            this.StartMatch = new RelayCommand(() => this.OnStartMatch());
        }

        private void OnStartMatch()
        {
            this.StartDone?.Invoke(this, EventArgs.Empty);
        }
    }
}
