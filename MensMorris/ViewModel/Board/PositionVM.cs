using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel.Board
{
    public class PositionVM
    {
        private BoardVM Parent;

        public BoardPosition Model { get; private set; }

        public Point Location { get; private set; }

        public PositionVM(BoardVM parent, BoardPosition model)
        {
            this.Parent = parent;
            this.Model = model;
            this.Location = PositionVM.CalculateLocation(this.Model.Ring, this.Model.Number);
        }

        private static Point CalculateLocation(int ring, int number)
        {
            Point location = new Point(50, 50);
            int radius = (4 - ring) * 15;
            switch (number)
            {
                case 0:
                    location.Offset(-radius, -radius);
                    break;
                case 1:
                    location.Offset(-radius, 0);
                    break;
                case 2:
                    location.Offset(-radius, radius);
                    break;
                case 3:
                    location.Offset(0, radius);
                    break;
                case 4:
                    location.Offset(radius, radius);
                    break;
                case 5:
                    location.Offset(radius, 0);
                    break;
                case 6:
                    location.Offset(radius, -radius);
                    break;
                case 7:
                    location.Offset(0, -radius);
                    break;
            }
            return location;
        }


    }
}
