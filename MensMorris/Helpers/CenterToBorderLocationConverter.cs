using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MensMorris.Game.Helpers
{
    /// <summary>
    /// Provides a converter to place elements via a margin
    /// </summary>
    public class CenterToBorderLocationConverter : IValueConverter
    {
        /// <summary>
        /// Converts an elements center point to an equivalent margin
        /// </summary>
        /// <param name="value">The center point where to place the element</param>
        /// <returns>A margin locating the element at the same position</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? string.Empty;
            if (value is Point && parameter is string)
            {
                Point center = (Point)value;
                // Extract the element size from the parameter
                Size size = CenterToBorderLocationConverter.extractParameter((string)parameter);
                return new Thickness(center.X - size.Width / 2, center.Y - size.Height / 2, 0, 0);
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

        private static Size extractParameter(string parameter)
        {
            IEnumerable<int> sizes = parameter
                .Split(new char[] { ' ' }, 2)
                .Select(str => parseInt(str))
                .Where(res => res.HasValue)
                .Select(res => res.Value);
            return new Size(sizes.FirstOrDefault(), sizes.LastOrDefault());
        }

        private static int? parseInt(string str)
        {
            int result;
            return Int32.TryParse(str, out result) ? new int?(result) : null;
        }
    }
}
