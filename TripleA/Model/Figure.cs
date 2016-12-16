using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Model
{
    public class Figure
    {
        public Point StartPoint { get; internal set; }
        public List<Point> Points { get; internal set; }

        public Figure()
        {

        }
    }
}