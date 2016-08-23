using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class ScreenMessageVM
    {
        public string Message { get; private set; }

        public bool IsActive
        {
            get
            {
                return this.Message != String.Empty;
            }
        }

        public ScreenMessageVM()
        {
            this.Message = String.Empty;
        }

        public void Set(String message)
        {
            this.Message = message;
        }

        public void Reset()
        {
            this.Message = String.Empty;
        }

    }
}
