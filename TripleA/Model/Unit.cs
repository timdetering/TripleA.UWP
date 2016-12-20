using GalaSoft.MvvmLight;

namespace TripleA.Model
{
    public class Unit : ViewModelBase
    {
        public Player Owner { get; internal set; }
        public UnitType Type { get; internal set; }
        public int Quantity { get; set; }
        private Point point;
        public Point Point
        {
            get { return point; }
            set { point = value; RaisePropertyChanged(); }
        }

        public Territory Territory { get; internal set; }
    }
}