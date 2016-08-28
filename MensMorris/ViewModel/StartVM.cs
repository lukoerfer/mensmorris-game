using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using PropertyChanged;
using GalaSoft.MvvmLight.CommandWpf;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for the start screen
    /// </summary>
    [ImplementPropertyChanged]
    public class StartVM
    {
        public event Action<SettingsVM, PlayerOptionVM, PlayerOptionVM> StartDone;

        public List<PlayerOptionVM> PlayerOptions { get; set; }

        public PlayerOptionVM FirstPlayer { get; set; }
        public PlayerOptionVM SecondPlayer { get; set; }

        public List<SettingsVM> SettingsOptions { get; set; }

        public SettingsVM Settings { get; set; }

        public ICommand StartMatch { get; set; }

        public StartVM()
        {
            // Init player options
            this.PlayerOptions = new List<PlayerOptionVM>();
            // Add the human player option
            this.PlayerOptions.Add(new PlayerOptionVM(null));
            // Add all available bots to the player options
            this.PlayerOptions.AddRange(BotCollector.CollectBots().Select(botType => new PlayerOptionVM(botType)));
            // Init rule options
            this.SettingsOptions = new List<SettingsVM>();
            // Add fixed game settings to the options
            this.SettingsOptions.AddRange(PredefinedGames.GetGames().Select(game => new SettingsVM(game.Key, true, game.Value)));
            // Add a free settings option
            this.SettingsOptions.Add(new SettingsVM("- Free choice -", false, new Settings(3, false, false, 9)));
            // Init start match button
            this.StartMatch = new RelayCommand(() => this.OnStartMatch());
        }

        private void OnStartMatch()
        {
            this.StartDone?.Invoke(this.Settings, this.FirstPlayer, this.SecondPlayer);
        }
    }
}
