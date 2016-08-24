using System;
using System.Collections.Generic;
using System.Linq;

using MensMorris.Engine;

namespace MensMorris.Bot
{
    public class RandomBot : IPlayer
    {
        private static Random Generator;

        static RandomBot()
        {
            RandomBot.Generator = new Random();
        }

        public RandomBot()
        {

        }

        public string GetName()
        {
            return "Random Bot";
        }

        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }

        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }

        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }
    }
}
