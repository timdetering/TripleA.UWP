using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Model
{
    public interface IGameDecisionMaker
    {
        void Decide(string step, Game data);
    }
}