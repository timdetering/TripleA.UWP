using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = TripleA.Model;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace TripleA.Converters
{
    public class TerritoryToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var territory = value as M.Territory;
            PathGeometry geom = new PathGeometry();

            foreach(var fig2 in territory.Figures)
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

        private Point ConvertToPoint(M.Point model)
        {
            var point = new Point();
            point.X = (double)model.X;
            point.Y = (double)model.Y;
            return point;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}