using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MensMorris.Engine;
using MensMorris.Bot;

namespace MensMorris.Game.ViewModel
{
    public class MatchVM : IPlayer
    {
        public event EventHandler MatchDone;

        private Match Match;

        public SlotVM FirstSlot { get; set; }

        public SlotVM SecondSlot { get; set; }

        public BoardVM Board { get; set; }

        public ScreenMessageVM ScreenMessage { get; set; }

        public MatchVM()
        {
            // Create a new MensMorris match
            this.Match = new Match(this, new RandomBot());
            this.Match.Finished += OnMatchFinished;
            // Init the slot view models
            this.FirstSlot = new SlotVM(this.Match.GetSlot(0));
            this.SecondSlot = new SlotVM(this.Match.GetSlot(1));
            // Init the board view model
            this.Board = new BoardVM(this.Match.GetBoard(), this.Match.GetTiles());
            // Init the screen message view model
            this.ScreenMessage = new ScreenMessageVM();
            // Start the match
            this.Match.Start();
        }

        private void OnMatchFinished(object sender, EventArgs e)
        {
            this.ScreenMessage.Set("Game finished");
        }

        public string GetName()
        {
            return "Player";
        }

        public PlaceAction SelectPlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            return possibleActions.First();
        }

        public MoveAction SelectMoveAction(List<MoveAction> possibleActions, Match match)
        {
            return possibleActions.First();
        }

        public KickAction SelectKickAction(List<KickAction> possibleActions, Match match)
        {
            return possibleActions.First();
        }
    }
}
