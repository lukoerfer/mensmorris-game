using System;
using System.Globalization;
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

        private String[] SlotColorNames = new String[]
        {
            "Red",
            "Blue"
        };

        private SolidColorBrush[] LightSlotColors = new SolidColorBrush[]
        {
            Brushes.Tomato,
            Brushes.RoyalBlue
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? string.Empty;
            if (value is int && parameter is string)
            {
                // Cast value and parameter
                int intValue = (int)value;
                string strParameter = (string)parameter;
                // Check for parameters
                if (strParameter.Equals("name"))
                {
                    return this.SlotColorNames[intValue];
                }
                if (strParameter.Equals("light")) {
                    return this.LightSlotColors[intValue];
                }
                return this.SlotColors[intValue];
            }
            else
            {
                throw new ArgumentException("Wrong value or parameter type");
            }
        }

        [Obsolete("Not supported")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
