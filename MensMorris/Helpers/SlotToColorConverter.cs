using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MensMorris.Game.Helpers
{
    public class SlotToColorConverter : IValueConverter
    {

        private static SolidColorBrush[] SlotColors = new SolidColorBrush[]
        {
            Brushes.Red,
            Brushes.Blue
        };

        private static String[] SlotColorNames = new String[]
        {
            "Red",
            "Blue"
        };

        private static SolidColorBrush[] LightSlotColors = new SolidColorBrush[]
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
                    return SlotToColorConverter.SlotColorNames[intValue];
                }
                if (strParameter.Equals("light")) {
                    return SlotToColorConverter.LightSlotColors[intValue];
                }
                return SlotToColorConverter.SlotColors[intValue];
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

        /// <summary>
        /// Additional slot to color conversion (in-code usage)
        /// </summary>
        public static Brush GetColor(int slot)
        {
            return SlotToColorConverter.SlotColors[slot];
        }

        /// <summary>
        /// Additional slot to color name conversion (in-code usage)
        /// </summary>
        public static string GetColorName(int slot)
        {
            return SlotToColorConverter.SlotColorNames[slot];
        }
    }
}
