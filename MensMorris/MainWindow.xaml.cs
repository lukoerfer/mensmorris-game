using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MensMorris.Engine;
using MensMorris.Game.ViewModel;

namespace MensMorris.Game
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPlayer
    {
        private Match Match;

        private BoardVM ViewModel;

        private Random Randomizer;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Match = new Match(this, this);
            this.ViewModel = new BoardVM(this.Match);
            this.DataContext = this.ViewModel;
            this.Randomizer = new Random();
            this.Match.Start();
        }

        public KickAction SelectKickAction(List<KickAction> possibleActions, Match match)
        {
            return possibleActions.Skip(this.Randomizer.Next(possibleActions.Count)).First();
        }

        public MoveAction SelectMoveAction(List<MoveAction> possibleActions, Match match)
        {
            return possibleActions.Skip(this.Randomizer.Next(possibleActions.Count)).First();
        }

        public PlaceAction SelectPlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            return possibleActions.Skip(this.Randomizer.Next(possibleActions.Count)).First();
        }
    }
}
