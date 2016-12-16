using System.Collections.Generic;

namespace TripleA.Model
{
    public class ProductionRule
    {
        public ProductionRule()
        {
        }

        public List<ProductionRuleCost> Costs { get; set; }
        public string Name { get; set; }
        public List<ProductionRuleResult> Results { get; set; }
    }
}