using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace TripleA.Model
{
    public class Territory : ViewModelBase
    {
        public bool IsWater { get; internal set; }
        public string Name { get; internal set; }
        public List<Territory> Connections { get; internal set; }
        public Player OriginalOwner { get; set; }
        public Player CurrentOwner { get; set; }
        public Point CenterPoint { get; internal set; }
        public List<Figure> Figures { get; set; }
        #region IsSelected
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value; this.RaisePropertyChanged();
            }
        }
        #endregion
        public List<Point> PlacementLocations { get; set; }
        public int Production { get; internal set; }
        public bool IsCapitol { get; internal set; }
        public Player CapitolOwner { get; internal set; }
        public bool IsVictoryCity { get; internal set; }
        public ObservableCollection<Unit> Units { get; set; }

        public Territory()
        {
            this.Connections = new List<Territory>();
            this.Figures = new List<Figure>();
            this.Units = new ObservableCollection<Unit>();
        }

        public void AddUnits(Unit unitsToAdd)
        {
            var match = (from x in Units
                         where
                         x.Type.Name == unitsToAdd.Type.Name &&
                         x.Owner.Name == unitsToAdd.Owner.Name &&
                         x.Territory.Name == unitsToAdd.Territory.Name
                         select x).FirstOrDefault();
            if (match == null)
            {
                this.Units.Add(unitsToAdd);
            }
            else
            {
                match.Quantity += unitsToAdd.Quantity;
            }
            RefreshUnitLocations();
        }

        public void RemoveUnits(Unit unitsToRemove)
        {
            var match = (from x in Units
                         where
                         x.Type.Name == unitsToRemove.Type.Name &&
                         x.Owner.Name == unitsToRemove.Owner.Name &&
                         x.Territory.Name == unitsToRemove.Territory.Name
                         select x).FirstOrDefault();
            if (match == null)
            {
                // can't do anything because nothing to remove!
            }
            else
            {
                match.Quantity -= unitsToRemove.Quantity;
            }
            RefreshUnitLocations();
        }
        private void RefreshUnitLocations()
        {
            if (this.PlacementLocations == null)
                return;

            int pointIndex = 0;
            foreach (var unit in this.Units.Where(f => f.Territory.Name == this.Name))
            {
                if (pointIndex < this.PlacementLocations.Count)
                {
                    unit.Point = this.PlacementLocations[pointIndex];
                }
                pointIndex++;
            }
        }
    }
}