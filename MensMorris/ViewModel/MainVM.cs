using System;

using PropertyChanged;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class MainVM
    {
        public object CurrentModel { get; set; }

        public MainVM()
        {
            // At startup show the start screen
            this.SetStart();
        }

        private void SetStart()
        {
            MatchVM oldVM = this.CurrentModel as MatchVM;
            if (oldVM != null) oldVM.MatchDone -= this.SetStart;
            StartVM newVM = new StartVM();
            newVM.StartDone += this.SetMatch;
            this.CurrentModel = newVM;
        }

        private void SetMatch(SettingsVM settings, PlayerOptionVM first, PlayerOptionVM second)
        {
            StartVM oldVM = this.CurrentModel as StartVM;
            if (oldVM != null) oldVM.StartDone -= this.SetMatch;
            MatchVM newVM = new MatchVM(settings, first, second);
            newVM.MatchDone += this.SetStart;
            this.CurrentModel = newVM;
        }
    }
}
