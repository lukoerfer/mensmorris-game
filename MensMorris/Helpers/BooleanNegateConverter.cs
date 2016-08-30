using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MensMorris.Game.Helpers
{
    /// <summary>
    /// Provides a converter to negate boolean values on XAML bindings
    /// </summary>
    public class BooleanNegateConverter : IValueConverter
    {
        /// <summary>
        /// Negates a boolean
        /// </summary>
        /// <param name="value">A boolean to negate</param>
        /// <returns>The negated boolean</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            else
            {
                throw new ArgumentException("Wrong value type");
            }
        }

        /// <remarks>
        /// Back conversion does absolutely the same
        /// </remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            else
            {
                throw new ArgumentException("Wrong value type");
            }
        }
    }
}
