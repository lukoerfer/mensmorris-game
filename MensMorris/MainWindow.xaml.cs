using System.Windows;
using MensMorris.Game.ViewModel;

namespace MensMorris.Game
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainVM();
        }
    }
}
