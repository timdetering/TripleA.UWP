using System;
using TripleA.Helpers;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using M = TripleA.Model;

namespace TripleA.Converters
{
    public class TerritoryToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var territory = value as M.Territory;
            var g = TerritoryGeometryHelper.InitializeGeometry(territory);
            return g;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}