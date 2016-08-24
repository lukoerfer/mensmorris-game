using System;
using System.Windows;
using System.Windows.Controls;

using MensMorris.Game.ViewModel;

namespace MensMorris.Game.Helpers
{
    public class BoardElementTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PositionTemplate { get; set; }

        public DataTemplate ConnectionTemplate { get; set; }

        public DataTemplate TileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConnectionVM)
            {
                return this.ConnectionTemplate;
            }
            else if (item is PositionVM)
            {
                return this.PositionTemplate;
            }
            else if (item is TileVM)
            {
                return this.TileTemplate;
            }
            else
            {
                throw new ArgumentException("Unknown item type");
            }
        }
    }
}
