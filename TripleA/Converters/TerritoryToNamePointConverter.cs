using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using M = TripleA.Model;

namespace TripleA.Converters
{
    public class TerritoryToNamePointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var coordinate = (string)parameter;
            var territory = value as M.Territory;
            var tb = new TextBlock { Text = territory.Name, FontSize = 10 };
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var w = tb.DesiredSize.Width;
            var h = tb.DesiredSize.Height;

            var x = territory.CenterPoint.X - (w / 2);
            var y = territory.CenterPoint.Y - (h / 2);

            switch (coordinate)
            {
                case "Y":
                    return y;
                case "X":
                    return x;
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}