using System.Collections.Generic;

namespace TripleA.Model
{
    public class Player
    {
        public Player()
        {
        }

        public string Color { get; internal set; }
        public bool IsOptional { get; internal set; }
        public string Name { get; internal set; }
        internal ProductionFrontier ProductionFrontier { get; set; }
        internal List<Resource> Resources { get; set; }
    }
}