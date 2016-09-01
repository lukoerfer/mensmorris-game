using System;
using System.Collections.Generic;
using System.Linq;

using MensMorris.Engine;

namespace MensMorris.Bot
{
    /// <summary>
    /// Implements a bot for a Mens Morris game deciding by pure chance
    /// </summary>
    public class RandomBot : IPlayer
    {
        // Use common random number generator for all random bots
        private static Random Generator;

        /// <summary>
        /// Static constructor
        /// </summary>
        static RandomBot()
        {
            // Init the random number generator
            RandomBot.Generator = new Random();
        }

        #region | Player interface | 

        public string GetName()
        {
            return "Random Bot";
        }

        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as PlaceAction;
        }

        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as MoveAction;
        }

        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as KickAction;
        }

        #endregion // Player interface

        private BaseAction ChooseAction(List<BaseAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }
    }
}
