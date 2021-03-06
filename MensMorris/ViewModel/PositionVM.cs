﻿using System.Windows;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for a board position
    /// </summary>
    public class PositionVM
    {
        private BoardVM Board;

        public BoardPosition Model { get; private set; }

        public Point Location { get; private set; }

        public PositionVM(BoardVM board, BoardPosition model)
        {
            this.Board = board;
            this.Model = model;
            this.Location = PositionVM.CalculateLocation(this.Model.Match.Settings.RingCount, this.Model.Ring, this.Model.Number);
        }

        private static Point CalculateLocation(int ringCount, int ring, int number)
        {
            Point location = new Point(50, 50);
            int radius = ring * (40 / ringCount);
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
