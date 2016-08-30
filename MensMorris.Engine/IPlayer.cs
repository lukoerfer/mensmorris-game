using System.Collections.Generic;

namespace MensMorris.Engine
{
    /// <summary>
    /// Defines an interface for Mens Morris players
    /// </summary>
    public interface IPlayer
    {
        string GetName();

        PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match);

        MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match);

        KickAction ChooseKickAction(List<KickAction> possibleActions, Match match);

    }
}
