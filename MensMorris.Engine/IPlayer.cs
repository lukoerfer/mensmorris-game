using System.Collections.Generic;

namespace MensMorris.Engine
{
    public interface IPlayer
    {
        string GetName();

        PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match);

        MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match);

        KickAction ChooseKickAction(List<KickAction> possibleActions, Match match);

    }
}
