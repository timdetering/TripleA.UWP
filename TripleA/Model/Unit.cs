using GalaSoft.MvvmLight;
using System;

namespace TripleA.Model
{
    public class Unit : ViewModelBase
    {
        public Player Owner { get; internal set; }
        public UnitType Type { get; internal set; }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; RaisePropertyChanged(); }
        }
        private Point point;
        public Point Point
        {
            get { return point; }
            set { point = value; RaisePropertyChanged(); }
        }

        public Territory Territory { get; internal set; }

        public Unit Clone()
        {
            var clone = new Unit();

            clone.Owner = this.Owner;
            clone.Type = this.Type;
            clone.Territory = this.Territory;
            clone.Quantity = 1;

            return clone;
        }
    }
}