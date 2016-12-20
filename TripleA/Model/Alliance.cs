using System.Collections.Generic;

namespace TripleA.Model
{
    public class Alliance
    {
        public string Name { get; internal set; }
        public List<Player> Players { get; internal set; }

        public Alliance()
        {
            this.Players = new List<Player>();
        }
    }
}