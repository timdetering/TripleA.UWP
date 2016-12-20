using System.Collections.Generic;

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