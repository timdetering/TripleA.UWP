using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Model;
using Windows.UI.Xaml.Data;

namespace TripleA.Converters
{
    public class UnitScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double unitScale = 1.0;
            if(Game.Instance.Map.Properties.ContainsKey("units.scale"))
            {
                double.TryParse(Game.Instance.Map.Properties["units.scale"], out unitScale);
            }
            return unitScale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}