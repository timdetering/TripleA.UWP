using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Model;

namespace TripleA.Attachments
{
    public class TerritoryAttachment : IAttachment
    {
        public void Apply(string attachTo, Dictionary<string, string> options)
        {
            var territory = Game.Instance.Map.Territories.Where(f => f.Name == attachTo).FirstOrDefault();
            if(territory != null)
            {
                if (options.ContainsKey("production"))
                {
                    territory.Production = int.Parse(options["production"]);
                }
                if (options.ContainsKey("capital"))
                {
                    var playerName = options["capital"];
                    territory.IsCapitol = true;
                    territory.CapitolOwner = Game.Instance.Players.Where(f => f.Name == playerName).FirstOrDefault();
                }
                if (options.ContainsKey("victoryCity"))
                {
                    territory.IsVictoryCity = bool.Parse(options["victoryCity"]);
                }
            }
            else
            {
                Debug.WriteLine("Missing this territory " + territory.Name);
            }
        }
    }
}