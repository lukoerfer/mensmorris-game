using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using PropertyChanged;

using MensMorris.Engine;

namespace MensMorris.Game.ViewModel
{
    [ImplementPropertyChanged]
    public class TileVM
    {
        private BoardVM Parent;

        [DoNotNotify]
        public Tile Model { get; private set; }

        [DoNotNotify]
        public int SlotNumber { get; private set; }

        public bool IsOnBoard { get; private set; }

        public Point Location { get; private set; }

        public ObservableCollection<TargetVM> RelatedTargets { get; set; }

        public bool IsSelectable
        {
            get
            {
                return this.RelatedTargets.Count > 0;
            }
        }

        public bool IsSelected { get; set; }

        public ICommand Select { get; set; }

        public TileVM(BoardVM parent, Tile model)
        {
            this.Parent = parent;
            this.Model = model;
            this.Model.AtChanged += OnAtChanged;
            this.SlotNumber = this.Model.Owner.ID;
            this.RelatedTargets = new ObservableCollection<TargetVM>();
            this.IsSelected = false;
            this.Select = new RelayCommand(() => this.SelectHandler());
        }

        private void OnAtChanged(object sender, EventArgs e)
        {
            this.IsOnBoard = this.Model.At != null;
            if (this.IsOnBoard) this.Location = this.Parent.GetPosition(this.Model.At).Location;
        }

        private void SelectHandler()
        {
            if (!this.IsSelected && this.IsSelectable)
            {
                this.Parent.SetSelectedTile(this.RelatedTargets.ToList());
                this.IsSelected = true;
            }
        }
    }
}
