using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = TripleA.Model;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace TripleA.Helpers
{
    public class TerritoryGeometryHelper
    {
        public static PathGeometry InitializeGeometry(M.Territory territory)
        {
            PathGeometry geom = new PathGeometry();

            foreach (var fig2 in territory.Figures)
            {
                PathFigure fig = new PathFigure();
                fig.IsClosed = true;
                fig.IsFilled = true;
                if (fig2.StartPoint == null)
                {
                    return null;
                }
                fig.StartPoint = ConvertToPoint(fig2.StartPoint);

                foreach (var point in fig2.Points)
                {
                    LineSegment line = new LineSegment();
                    line.Point = ConvertToPoint(point);
                    fig.Segments.Add(line);
                }

                geom.Figures.Add(fig);
            }
            return geom;
        }

        private static Point ConvertToPoint(M.Point model)
        {
            var point = new Point();
            point.X = (double)model.X;
            point.Y = (double)model.Y;
            return point;
        }
    }
}