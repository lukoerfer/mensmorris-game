using System;

using PropertyChanged;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// Main application view model
    /// </summary>
    [ImplementPropertyChanged]
    public class MainVM
    {
        /// <summary>
        /// Gets or sets the current view model
        /// </summary>
        public object CurrentModel { get; set; }

        /// <summary>
        /// Creates a new main application view model
        /// </summary>
        public MainVM()
        {
            // At startup show the start screen
            this.SetStart();
        }

        /// <summary>
        /// Change the current view to the start screen
        /// </summary>
        private void SetStart()
        {
            // Cleanup the previous Match view model
            MatchVM oldVM = this.CurrentModel as MatchVM;
            if (oldVM != null) oldVM.MatchDone -= this.SetStart;
            // Create and set new Start view model
            StartVM newVM = new StartVM();
            newVM.StartDone += this.SetMatch;
            this.CurrentModel = newVM;
        }

        /// <summary>
        /// Change the current view to the match screen
        /// </summary>
        /// <param name="settings">The chosen match settings</param>
        /// <param name="first">The first player choice</param>
        /// <param name="second">The second player choice</param>
        private void SetMatch(SettingsVM settings, PlayerOptionVM first, PlayerOptionVM second)
        {
            // Cleanup the previous Start view model
            StartVM oldVM = this.CurrentModel as StartVM;
            if (oldVM != null) oldVM.StartDone -= this.SetMatch;
            // Create and set new Match view model
            MatchVM newVM = new MatchVM(settings, first, second);
            newVM.MatchDone += this.SetStart;
            this.CurrentModel = newVM;
        }
    }
}
