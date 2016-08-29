using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class SettingsVM
    {
        public string Name { get; set; }

        public bool IsFixed { get; set; }

        public int RingCount { get; set; }

        public bool CenterPoint { get; set; }

        public bool CenterCross { get; set; }

        public bool ConnectCorners { get; set; }

        public int TilesPerSlot { get; set; }

        public int MaxTilesPerSlot
        {
            get
            {
                int maxTiles = this.RingCount * 4 - 1;
                if (this.TilesPerSlot > maxTiles) this.TilesPerSlot = maxTiles;
                return maxTiles;
            }
        }

        public SettingsVM(string name, bool isFixed, Settings settings)
        {
            this.Name = name;
            this.IsFixed = isFixed;
            // Extract the single rules
            this.RingCount = settings.RingCount;
            this.CenterPoint = settings.CenterPoint;
            this.CenterCross = settings.CenterCross;
            this.ConnectCorners = settings.ConnectCorners;
            this.TilesPerSlot = settings.TilesPerSlot;
        }

        /// <summary>
        /// Returns the user- or predefined settings
        /// </summary>
        /// <returns></returns>
        public Settings ExtractSettings()
        {
            return new Settings(this.RingCount, this.CenterPoint, this.CenterCross, this.ConnectCorners, this.TilesPerSlot);
        }

        /// <summary>
        /// Returns the name as string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
