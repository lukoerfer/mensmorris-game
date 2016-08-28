using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MensMorris.Engine;

namespace MensMorris.Bot
{
    public class HeuristicBot : IPlayer
    {
        public string GetName()
        {
            return "Heuristic Bot";
        }

        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            throw new NotImplementedException();
        }

        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            throw new NotImplementedException();
        }

        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            throw new NotImplementedException();
        }
    }
}
