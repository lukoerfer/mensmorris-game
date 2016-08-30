using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MensMorris.Game.Helpers
{
    /// <summary>
    /// Provides the color encoding for different player slots
    /// </summary>
    public class SlotToColorConverter : IValueConverter
    {
        private const string NAME = "name";
        private const string LIGHT = "light";

        /// <summary>
        /// Stores the basic slot colors
        /// </summary>
        private static SolidColorBrush[] SlotColors = new SolidColorBrush[]
        {
            Brushes.Red,
            Brushes.Blue
        };

        /// <summary>
        /// Stores the names of the slot colors
        /// </summary>
        private static String[] SlotColorNames = new String[]
        {
            "Red",
            "Blue"
        };

        /// <summary>
        /// Stores lighter variations of the slot colors
        /// </summary>
        private static SolidColorBrush[] LightSlotColors = new SolidColorBrush[]
        {
            Brushes.Tomato,
            Brushes.RoyalBlue
        };
        
        /// <summary>
        /// Converts the slot number to the related color, color name or lighter color
        /// </summary>
        /// <param name="value">The slot number</param>
        /// <returns>The slot color, color name or lighter color</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? string.Empty;
            if (value is int && parameter is string)
            {
                // Cast value and parameter
                int intValue = (int)value;
                string strParameter = (string)parameter;
                // Check for parameters
                if (strParameter.Equals(SlotToColorConverter.NAME))
                {
                    return SlotToColorConverter.SlotColorNames[intValue];
                }
                if (strParameter.Equals(SlotToColorConverter.LIGHT)) {
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
