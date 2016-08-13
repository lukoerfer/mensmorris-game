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
    public class CenterToBorderLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point && parameter is String)
            {
                Point center = (Point)value;
                Size size = CenterToBorderLocationConverter.extractParameter((String)parameter);
                Thickness border = new Thickness(center.X - size.Width / 2, center.Y - size.Height / 2, 0, 0);
                return border;
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

        private static Size extractParameter(String parameter)
        {
            String[] sizeStrings = parameter.Split(new char[] { ' ' }, 2);
            IEnumerable<int> sizes = sizeStrings.Select(str => parseInt(str)).Where(res => res.HasValue).Select(res => res.Value);
            return new Size(sizes.FirstOrDefault(), sizes.LastOrDefault());
        }

        private static int? parseInt(String str)
        {
            int result;
            return Int32.TryParse(str, out result) ? new int?(result) : null;
        }
    }
}
