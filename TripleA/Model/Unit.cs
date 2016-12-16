using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Model
{
    public class Unit : ViewModelBase
    {
        public Player Owner { get; internal set; }
        public UnitType Type { get; internal set; }
        public int Quantity { get; set; }
        private Point point;
        public Point Point
        {
            get { return point; }
            set { point = value; RaisePropertyChanged(); }
        }

        public Territory Territory { get; internal set; }
    }
}