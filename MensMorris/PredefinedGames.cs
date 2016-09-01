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
            rules.Add("Nine Men's Morris", new Settings(3, false, false, false, 9));
            rules.Add("Nine Holes", new Settings(1, true, false, true, 3));
            rules.Add("Five Men's Morris", new Settings(2, false, false, false, 5));
            rules.Add("Six Men's Morris", new Settings(2, false, false, false, 6));
            rules.Add("Seven Men's Morris", new Settings(2, false, true, false, 7));
            rules.Add("Eleven Men's Morris", new Settings(3, false, false, true, 11));
            rules.Add("Twelve Men's Morris", new Settings(3, false, false, true, 12));
            return rules;
        }

    }
}
