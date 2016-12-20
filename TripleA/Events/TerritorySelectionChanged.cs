using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Model;

namespace TripleA.Events
{
    public class TerritorySelectionChanged
    {
        public Territory Territory { get; set; }
    }
}