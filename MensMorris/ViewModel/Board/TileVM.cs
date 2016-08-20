using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel.Board
{
    [ImplementPropertyChanged]
    public class TileVM
    {
        private BoardVM Parent;

        [DoNotNotify]
        public Tile Model { get; private set; }

        public bool IsOnBoard { get; private set; }

        public Point Location { get; private set; }

        public int Slot { get; private set; }

        public TileVM(BoardVM parent, Tile model)
        {
            this.Parent = parent;
            this.Model = model;
            this.Model.AtChanged += OnAtChanged;
            this.Slot = this.Model.Owner.ID;
        }

        private void OnAtChanged(object sender, EventArgs e)
        {
            this.IsOnBoard = this.Model.At != null;
            if (this.IsOnBoard) this.Location = this.Parent.GetPosition(this.Model.At.Ring, this.Model.At.Number).Location;
        }
    }
}
