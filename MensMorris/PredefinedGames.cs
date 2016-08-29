using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MensMorris.Engine;

namespace MensMorris.Game
{
    public static class PredefinedGames
    {
        public static Dictionary<string, Settings> GetGames()
        {
            Dictionary<string, Settings> rules = new Dictionary<string, Settings>();
            rules.Add("Nine Mens Morris", new Settings(3, false, false, false, 9));
            rules.Add("Nine Holes", new Settings(1, true, false, true, 3));
            rules.Add("Six Mens Morris", new Settings(2, false, false, false, 6));
            return rules;
        }

    }
}
