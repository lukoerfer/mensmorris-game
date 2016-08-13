using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensMorris.Engine
{
    public interface IPlayer
    {
        PlaceAction SelectPlaceAction(List<PlaceAction> possibleActions, Match match);

        MoveAction SelectMoveAction(List<MoveAction> possibleActions, Match match);

        KickAction SelectKickAction(List<KickAction> possibleActions, Match match);

    }
}
