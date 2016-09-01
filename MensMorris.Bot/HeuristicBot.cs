using System;
using System.Collections.Generic;
using System.Linq;

using MensMorris.Engine;

namespace MensMorris.Bot
{
    /// <summary>
    /// Implements a bot for a Mens Morris game deciding with the help of heuristic evaluation functions
    /// </summary>
    public class HeuristicBot : IPlayer
    {
        private static int[] PlaceCoefficients = new int[] { 50, 1, 26, 6, 12, 6, 7 };
        private static int[] MoveCoefficients = new int[] { 50, 1, 43, 6, 10, 8, 7, 16 };
        private static int[] FlyCoefficients = new int[] { 50, 1, 10, 1 };

        private static Func<Slot, int>[] SlotHeuristics = new Func<Slot, int>[]
        {
            slot => slot.PossibleMoveCount(),
            slot => slot.TwoPieceConfigurationCount(),
            slot => slot.ThreePieceConfigurationCount(),
            slot => slot.MorrisCount(),
            slot => slot.BlockedTileCount(),
            slot => slot.GetTilesOnBoard().Count,
            slot => slot.DoubleMorrisCount()
        };

        private static Func<Slot, int>[] PlaceHeuristics
        {
            get
            {
                return SlotHeuristics.Take(6).ToArray();
            }
        }

        private static Func<Slot, int>[] MoveHeuristics
        {
            get
            {
                return SlotHeuristics;
            }
        }

        private static Func<Slot, int>[] FlyHeuristics
        {
            get
            {
                return SlotHeuristics.Take(3).ToArray();
            }
        }

        #region | Player interface |

        public string GetName()
        {
            return "Heuristic Bot";
        }

        public PlaceAction ChoosePlaceAction(List<PlaceAction> possibleActions, Match match)
        {
            // Determine the best PlaceAction
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as PlaceAction;
        }

        public MoveAction ChooseMoveAction(List<MoveAction> possibleActions, Match match)
        {
            // Determine the best MoveAction
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as MoveAction;
        }

        public KickAction ChooseKickAction(List<KickAction> possibleActions, Match match)
        {
            // Determine the best KickAction
            return this.ChooseAction(possibleActions.Cast<BaseAction>().ToList(), match) as KickAction;
        }

        #endregion // Player interface

        private BaseAction ChooseAction(List<BaseAction> possibleActions, Match match)
        {
            Dictionary<BaseAction, int> evaluation = new Dictionary<BaseAction, int>();
            foreach (BaseAction action in possibleActions)
            {
                // Simulate the action in this scope
                using (match.SimulateAction(action))
                {
                    if (action is PlaceAction)
                    {
                        evaluation.Add(action, this.EvaluateBoardPlace(action));
                    }
                    if (action is MoveAction)
                    {
                        if (!action.Executing.CanFly)
                        {
                            evaluation.Add(action, this.EvaluateBoardMove(action));
                        }
                        else
                        {
                            evaluation.Add(action, this.EvaluateBoardFly(action));
                        }
                    }
                    if (action is KickAction)
                    {
                        if (match.Phase == GamePhase.PlacingPhase)
                        {
                            evaluation.Add(action, this.EvaluateBoardPlace(action));
                        }
                        if (match.Phase == GamePhase.MovingPhase)
                        {
                            if (!action.Executing.CanFly)
                            {
                                evaluation.Add(action, this.EvaluateBoardMove(action));
                            }
                            else
                            {
                                evaluation.Add(action, this.EvaluateBoardFly(action));
                            }
                        }
                    }
                }
            }
            // Return the action with the highest evaluated value
            return evaluation.OrderByDescending(eval => eval.Value).First().Key;
        }

        private int EvaluateBoardPlace(BaseAction action)
        {
            int baseValue = 0;
            if (action is PlaceAction)
            {
                baseValue = PlaceCoefficients[0] * ((action as PlaceAction).Tile.FormsMill() ? 1 : 0);
            }
            // Extract the own and the opponent match slot
            Slot me = action.Executing, opponent = me.GetOpponent();
            return HeuristicBot.PlaceHeuristics
                .Select((property, index) => PlaceCoefficients[index + 1] * (property(me) - property(opponent)))
                .Aggregate(baseValue, (x, y) => x + y);
        }

        private int EvaluateBoardMove(BaseAction action)
        {
            int baseValue = 0;
            if (action is MoveAction)
            {
                baseValue = MoveCoefficients[0] * ((action as MoveAction).Tile.FormsMill() ? 1 : 0);
            }
            // Extract the own and the opponent match slot
            Slot me = action.Executing, opponent = me.GetOpponent();
            return HeuristicBot.MoveHeuristics
                .Select((property, index) => MoveCoefficients[index + 1] * (property(me) - property(opponent)))
                .Aggregate(baseValue, (x, y) => x + y);
        }

        private int EvaluateBoardFly(BaseAction action)
        {
            int baseValue = 0;
            if (action is MoveAction)
            {
                baseValue = FlyCoefficients[0] * ((action as MoveAction).Tile.FormsMill() ? 1 : 0);
            }
            // Extract the own and the opponent match slot
            Slot me = action.Executing, opponent = me.GetOpponent();
            return HeuristicBot.FlyHeuristics
                .Select((property, index) => FlyCoefficients[index + 1] * (property(me) - property(opponent)))
                .Aggregate(baseValue, (x, y) => x + y);
        }

    }

    public static class HeuristicBotHelpers
    {
        public static int PossibleMoveCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Select(tile => tile.At.GetNeighbors().Count(neigh => neigh.IsFree))
                .Aggregate(0, (free1, free2) => free1 + free2);
        }

        public static int BlockedTileCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Count(tile => !tile.At.GetNeighbors()
                    .Any(neigh => neigh.IsFree));
        }

        public static int MorrisCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Select(tile => tile.MorrisCount())
                .Aggregate(0, (c1, c2) => c1 + c2) / 3;
        }

        public static int MorrisCount(this Tile tile)
        {
            return DirectionHelpers.Enumerate()
                .Where(dir => tile.Owner.DoesOwn(tile.At.Neighbor(dir)) &&
                (tile.Owner.DoesOwn(tile.At.Neighbor(dir.Opposite())) || tile.Owner.DoesOwn(tile.At.Neighbor(dir).Neighbor(dir))))
                .Select(dir => dir.ToMain())
                .Distinct()
                .Count();
        }

        public static int DoubleMorrisCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Select(tile => tile.MorrisCount())
                .Count(millCount => millCount > 1);
        }

        public static int TwoPieceConfigurationCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Select(tile => tile.TwoPieceConfigurationCount())
                .Aggregate(0, (c1, c2) => c1 + c2) / 2;
        }

        public static int TwoPieceConfigurationCount(this Tile tile)
        {
            return DirectionHelpers.Enumerate()
                .Where(dir => tile.Owner.DoesOwn(tile.At.Neighbor(dir)) &&
                ((tile.At.Neighbor(dir.Opposite()) != null) && tile.At.Neighbor(dir.Opposite()).IsFree 
                || (tile.At.Neighbor(dir).Neighbor(dir) != null && tile.At.Neighbor(dir).Neighbor(dir).IsFree)))
                .Select(dir => dir.ToMain())
                .Distinct()
                .Count();
        }
        
        public static int ThreePieceConfigurationCount(this Slot slot)
        {
            return slot.GetTilesOnBoard()
                .Where(tile => tile.NeighborOwnCount() >= 2)
                .Count();
        }

        public static int NeighborOwnCount(this Tile tile)
        {
            return DirectionHelpers.Enumerate()
                .Where(dir => tile.Owner.DoesOwn(tile.At.Neighbor(dir)))
                .Count();
        }
    }
}
