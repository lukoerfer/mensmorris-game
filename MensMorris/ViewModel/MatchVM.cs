using System;
using System.Collections.Generic;
using System.Threading;

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
            // Init the cross-thread waiter
            this.ActionWaiter = new AutoResetEvent(false);
            // Init the board view model
            this.Board = new BoardVM(this.Match.GetBoard(), this.Match.GetTiles());
            this.Board.ActionChosen += OnActionChosen;
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

        private AutoResetEvent ActionWaiter;

        private BaseAction ChosenAction;

        private void OnActionChosen(object sender, BaseAction chosenAction)
        {
            this.ChosenAction = chosenAction;
            this.ActionWaiter.Set();
        }

        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            this.Board.SetPlaceActions(possibleActions);
            this.ActionWaiter.WaitOne();
            return this.ChosenAction as PlaceAction;
        }

        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            this.Board.SetMoveActions(possibleActions);
            this.ActionWaiter.WaitOne();
            return this.ChosenAction as MoveAction;
        }

        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            this.Board.SetKickActions(possibleActions);
            this.ActionWaiter.WaitOne();
            return this.ChosenAction as KickAction;
        }
    }
}
