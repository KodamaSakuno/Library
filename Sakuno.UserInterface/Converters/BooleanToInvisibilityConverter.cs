using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.Converters
{
    public class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rBoolean = false;

            if (rpValue is bool)
                rBoolean = (bool)rpValue;
            else if (rpValue is bool?)
            {
                var rNullableBoolean = (bool?)rpValue;
                rBoolean = rNullableBoolean.HasValue && rNullableBoolean.Value;
            }

            return rBoolean ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture) =>
            rpValue is Visibility ? (Visibility)rpValue == Visibility.Collapsed : false;
    }
}
