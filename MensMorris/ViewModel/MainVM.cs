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
            StartVM startVM = new StartVM();
            startVM.StartDone += OnStartDone;
            this.CurrentModel = startVM;
        }

        private void OnStartDone(object sender, EventArgs e)
        {
            (this.CurrentModel as StartVM).StartDone -= OnStartDone;
            this.SetMatch();
        }

        private void SetMatch()
        {
            MatchVM matchVM = new MatchVM();
            matchVM.MatchDone += OnMatchDone;
            this.CurrentModel = matchVM;
        }

        private void OnMatchDone(object sender, EventArgs args)
        {
            
        }
    }
}
