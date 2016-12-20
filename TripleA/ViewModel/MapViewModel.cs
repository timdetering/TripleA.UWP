using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Model;

namespace TripleA.ViewModel
{
    public class MapViewModel
    {
        public Map Map
        {
            get
            {
                return Game.Instance.Map;
            }
        }

        public async Task Initialize()
        {
            await Game.Instance.Initialize("Classic", "classic.xml");
            //await Game.Instance.Initialize("WaW", "World_At_War.xml");
        }
    }
}