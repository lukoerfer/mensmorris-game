using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using PropertyChanged;

using MensMorris.Engine;
using System.Collections.Generic;

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

        public bool IsSelectable { get; set; }

        public bool IsSelected { get; set; }

        public ICommand Select { get; set; }

        public TileVM(BoardVM parent, Tile model)
        {
            this.Parent = parent;
            this.Model = model;
            this.Model.AtChanged += OnAtChanged;
            this.SlotNumber = this.Model.Owner.ID;
            this.IsSelectable = false;
            this.RelatedTargets = new ObservableCollection<TargetVM>();
            this.RelatedTargets.CollectionChanged += OnRelatedTargetsChanged;
            this.IsSelected = false;
            this.Select = new RelayCommand(() => this.SelectHandler());
        }

        private void OnRelatedTargetsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.IsSelectable = this.RelatedTargets.Count > 0;
        }

        private void OnAtChanged(object sender, EventArgs e)
        {
            this.IsOnBoard = this.Model.At != null;
            if (this.IsOnBoard) this.Location = this.Parent.GetPosition(this.Model.At).Location;
        }

        private void SelectHandler()
        {
            if (this.IsSelectable)
            {
                bool wasSelected = this.IsSelected;
                this.Parent.SetSelectedTile(wasSelected 
                    ? new List<TargetVM>() : this.RelatedTargets.ToList());
                this.IsSelected = !wasSelected;
            }
        }
    }
}
