using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

using PropertyChanged;

using MensMorris.Engine;
using GalaSoft.MvvmLight.CommandWpf;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for the match screen
    /// </summary>
    [ImplementPropertyChanged]
    public class MatchVM : IPlayer
    {
        /// <summary>
        /// Notifies about a done match without a rematch
        /// </summary>
        public event Action MatchDone;

        private Match Match;

        /// <summary>
        /// 
        /// </summary>
        public ICommand BackToMenu { get; set; }

        public SlotVM FirstSlot { get; set; }

        public SlotVM SecondSlot { get; set; }

        public BoardVM Board { get; set; }

        public ScreenMessageVM ScreenMessage { get; set; }

        private SettingsVM Settings;
        private PlayerOptionVM First, Second;

        public MatchVM(SettingsVM settings, PlayerOptionVM first, PlayerOptionVM second)
        {
            // Remember match info
            this.Settings = settings; this.First = first; this.Second = second;
            // Init the match
            this.Initialize();
        }

        private void Initialize()
        {
            // Create a new MensMorris match
            this.Match = new Match(this.Settings.ExtractSettings(), this.ExtractPlayer(this.First), this.ExtractPlayer(this.Second));
            this.Match.WaitTime = new TimeSpan(0, 0, 0, 0, 500);
            this.Match.Finished += OnMatchFinished;
            // Init the back to menu button
            this.BackToMenu = new RelayCommand(this.GoBackToMenu);
            // Init the slot view models
            this.FirstSlot = new SlotVM(this.Match.GetSlot(0));
            this.SecondSlot = new SlotVM(this.Match.GetSlot(1));
            // Init the cross-thread waiter
            this.ActionWaiter = new AutoResetEvent(false);
            // Init the board view model
            this.Board = new BoardVM(this.Match.GetBoard(), this.Match.GetTiles());
            this.Board.ActionChosen += this.OnActionChosen;
            // Init the screen message view model
            this.ScreenMessage = new ScreenMessageVM();
            // Start the match
            this.Match.Start();
        }

        /// <summary>
        /// Extracts a player instance from a selected player option
        /// </summary>
        /// <param name="playerOption">The selected player option</param>
        /// <returns>Either a created player or this view model for human players</returns>
        private IPlayer ExtractPlayer(PlayerOptionVM playerOption)
        {
            return (playerOption.PlayerType == null) 
                ? this : (IPlayer)Activator.CreateInstance(playerOption.PlayerType);
        }

        /// <summary>
        /// Handler for match completion
        /// </summary>
        private void OnMatchFinished(object sender, EventArgs e)
        {
            // Remove the event handler
            this.Match.Finished -= this.OnMatchFinished;
            // Determine winner slot number
            int winnerNumber = this.Match.GetWinnerSlot().ID;
            // Prepare the screen message options
            ScreenMessageOptionVM rematchOption = new ScreenMessageOptionVM("Rematch", this.StartRematch);
            ScreenMessageOptionVM menuOption = new ScreenMessageOptionVM("Back to menu", this.GoBackToMenu);
            // Show the screen message
            this.ScreenMessage.Set(
                Helpers.SlotToColorConverter.GetColorName(winnerNumber) + " wins", 
                Helpers.SlotToColorConverter.GetColor(winnerNumber),
                rematchOption, menuOption);
        }

        /// <summary>
        /// Restarts the match with the same players and settings
        /// </summary>
        private void StartRematch()
        {
            // Hide the screen message
            this.ScreenMessage.Reset();
            // Initialize a new match
            this.Initialize();
        }

        /// <summary>
        /// Returns to the start menu
        /// </summary>
        private void GoBackToMenu()
        {
            // Remove the event handler
            this.Match.Finished -= this.OnMatchFinished;
            // Stop the match if not stopped already
            this.Match.Stop();
            // Release the waiting game thread
            this.ActionWaiter.Set();
            // Notify the main view model
            this.MatchDone?.Invoke();
        }

        #region | Player interface |

        public string GetName()
        {
            // Returns the human player name
            return "Player";
        }

        private AutoResetEvent ActionWaiter;
        private BaseAction ChosenAction;

        /// <summary>
        /// Handles an action choice on the board
        /// </summary>
        /// <param name="chosenAction">The chosen action</param>
        private void OnActionChosen(BaseAction chosenAction)
        {
            this.ChosenAction = chosenAction;
            // Notify the waiting game thread to continue
            this.ActionWaiter.Set();
        }

        /// <summary>
        /// Lets the user choose a place action
        /// </summary>
        /// <param name="possibleActions">The list of possible place actions</param>
        /// <returns>The chosen place action</returns>
        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            // Show the actions on the board
            this.Board.SetPlaceActions(possibleActions);
            // Wait for user to choose an action
            this.ActionWaiter.WaitOne();
            // Return the chosen action
            return this.ChosenAction as PlaceAction;
        }

        /// <summary>
        /// Lets the user choose a move action
        /// </summary>
        /// <param name="possibleActions">The list of possible move actions</param>
        /// <returns>The chosen move action</returns>
        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            // Show the actions on the board
            this.Board.SetMoveActions(possibleActions);
            // Wait for user to choose an action
            this.ActionWaiter.WaitOne();
            // Return the chosen action
            return this.ChosenAction as MoveAction;
        }

        /// <summary>
        /// Lets the user choose a kick action
        /// </summary>
        /// <param name="possibleActions">The list of possible kick actions</param>
        /// <returns>The chosen kick action</returns>
        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            // Show the actions on the board
            this.Board.SetKickActions(possibleActions);
            // Wait for user to choose an action
            this.ActionWaiter.WaitOne();
            // Return the chosen action
            return this.ChosenAction as KickAction;
        }

        #endregion
    }
}
