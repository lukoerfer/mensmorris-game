using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public String GetName()
        {
            return "Random Bot";
        }

        public KickAction SelectKickAction(List<KickAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }

        public MoveAction SelectMoveAction(List<MoveAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }

        public PlaceAction SelectPlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            return possibleActions
                .Skip(RandomBot.Generator.Next(possibleActions.Count)) // Skip a random amount of moves
                .First();
        }
    }
}
