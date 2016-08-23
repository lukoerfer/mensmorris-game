using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MensMorris.Game.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private const String COLLAPSED = "collapse";
        private const String INVERT = "invert";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? String.Empty;
            if (value is bool && parameter is string)
            {
                String parameters = (string)parameter;
                bool isVisible = (bool)value;
                if (parameters.Contains(INVERT)) isVisible = !isVisible;
                Visibility notShown = parameters.Contains(COLLAPSED) ? Visibility.Collapsed : Visibility.Hidden;
                return isVisible ? Visibility.Visible : notShown;
            }
            else
            {
                throw new ArgumentException("Wrong value or parameter type.");
            }
        }

        [Obsolete("Not supported")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
