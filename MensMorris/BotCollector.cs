using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MensMorris.Engine;
using MensMorris.Bot;

namespace MensMorris.Game
{
    public static class BotCollector
    {
        public static List<Type> CollectBots()
        {
            return new Type[]
            {
                typeof(RandomBot)
            }.ToList();
        }
    }
}
