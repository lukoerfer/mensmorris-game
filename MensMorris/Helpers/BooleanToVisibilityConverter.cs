using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MensMorris.Game.Helpers
{
    /// <summary>
    /// Provides a converter to show, hide or collapse elements via a boolean binding
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private const string COLLAPSED = "collapse";
        private const string INVERT = "invert";

        /// <summary>
        /// Converts a boolean to a visibility regarding various parameters
        /// </summary>
        /// <param name="value">A boolean whether to show an element</param>
        /// <returns>A Visibility, either Visible, Hidden or Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? string.Empty;
            if (value is bool && parameter is string)
            {
                string parameters = (string)parameter;
                bool isVisible = (bool)value;
                // Invert input on special parameter
                if (parameters.Contains(BooleanToVisibilityConverter.INVERT)) isVisible = !isVisible;
                // Determine the visibility to use, when the input is false
                Visibility notShown = parameters.Contains(BooleanToVisibilityConverter.COLLAPSED) 
                    ? Visibility.Collapsed : Visibility.Hidden;
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
