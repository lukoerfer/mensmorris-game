using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class MainVM
    {
        public object CurrentContent { get; set; }

        public MainVM()
        {
            this.SetStart();
        }

        private void SetStart()
        {
            StartVM vm = new StartVM();
            this.CurrentContent = vm;
        }

        private void SetMatch()
        {
            MatchVM vm = new MatchVM();
            this.CurrentContent = vm;
        }


    }
}
