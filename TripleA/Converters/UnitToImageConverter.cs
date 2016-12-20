using System;
using TripleA.Model;
using Windows.UI.Xaml.Data;

namespace TripleA.Converters
{
    public class UnitToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var unit = value as Unit;
            var defaultPath = "ms-appx:///Game/assets/units/" + unit.Owner.Name + "/" + unit.Type.Name + ".png";

            if(Game.Instance.HasCustomUnitGraphics)
            {
                defaultPath = "ms-appx:///Game/" + Game.Instance.Scenario + "/units/" + unit.Owner.Name + "/" + unit.Type.Name + ".png";
            }

            return defaultPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}