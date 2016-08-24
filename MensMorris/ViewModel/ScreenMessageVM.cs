using PropertyChanged;

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
                return this.Message != string.Empty;
            }
        }

        public ScreenMessageVM()
        {
            this.Message = string.Empty;
        }

        public void Set(string message)
        {
            this.Message = message;
        }

        public void Reset()
        {
            this.Message = string.Empty;
        }

    }
}
