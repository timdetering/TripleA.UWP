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

        public override bool Equals(object obj)
        {
            var isEqual = false;
            if (obj != null && obj is Player)
            {
                var otherPlayer = obj as Player;
                isEqual = Name.Equals(otherPlayer.Name);
            }
            return isEqual;
        }
    }
}