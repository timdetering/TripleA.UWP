using System.Collections.Generic;

namespace TripleA.Model
{
    internal class ProductionFrontier
    {
        public ProductionFrontier()
        {
        }

        public string Name { get; internal set; }
        public List<ProductionRule> Rules { get; internal set; }
    }
}