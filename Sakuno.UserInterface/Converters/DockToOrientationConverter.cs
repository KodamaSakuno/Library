using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    [ValueConversion(typeof(Dock), typeof(Orientation))]
    public class DockToOrientationConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rDock = (Dock)rpValue;
            return rDock == Dock.Left || rDock == Dock.Right ? Orientation.Horizontal : Orientation.Vertical;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
