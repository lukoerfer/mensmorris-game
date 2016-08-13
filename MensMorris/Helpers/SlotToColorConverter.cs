using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MensMorris.Game.Helpers
{
    public class SlotToColorConverter : IValueConverter
    {

        private SolidColorBrush[] SlotColors = new SolidColorBrush[]
        {
            Brushes.Red,
            Brushes.Blue
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return this.SlotColors[(int)value];
            }
            else
            {
                throw new ArgumentException("Wrong value type. Must be int.");
            }
        }

        [Obsolete("Not supported")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
