using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TripleA.Events;

namespace TripleA.Model
{
    public class Map : ViewModelBase
    {

        private Territory selectedTerritory;

        public Territory SelectedTerritory
        {
            get { return selectedTerritory; }
            set
            {
                selectedTerritory = value;
                var otherTerritories = this.Territories.ToList();
                otherTerritories.Remove(selectedTerritory);
                foreach (var t in otherTerritories)
                {
                    t.IsSelected = false;
                }
                if(selectedTerritory != null)
                {
                    selectedTerritory.IsSelected = true;
                }

                var payload = new TerritorySelectionChanged();
                payload.Territory = selectedTerritory;
                Messenger.Default.Send<TerritorySelectionChanged>(payload);

                this.RaisePropertyChanged();
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public List<Territory> Territories { get; internal set; }
        public Dictionary<string, string> Properties { get; internal set; }
        public bool HasRelief { get; internal set; }
        public int RowCount { get; internal set; }
        public int ColumnCount { get; internal set; }
        public ObservableCollection<Tile> BaseTiles { get; internal set; }
        public ObservableCollection<Tile> ReliefTiles { get; internal set; }
        public int TileHeight { get; internal set; }
        public int TileWidth { get; internal set; }
        public List<Territory> WaterTerritories
        {
            get
            {
                var t = from x in this.Territories
                        where x.OriginalOwner != null && x.OriginalOwner.Name == "Water"
                        select x;
                return t.ToList();
            }
        }
        public List<Territory> NonWaterTerritories
        {
            get
            {
                var t = from x in this.Territories
                        where x.OriginalOwner != null && x.OriginalOwner.Name != "Water"
                        select x;
                return t.ToList();
            }
        }
        public ObservableCollection<Capitol> Capitols { get; private set; }

        public Map()
        {
            this.Territories = new List<Territory>();
            this.Properties = new Dictionary<string, string>();
            this.BaseTiles = new ObservableCollection<Tile>();
            this.ReliefTiles = new ObservableCollection<Tile>();
            this.Capitols = new ObservableCollection<Capitol>();
        }
    }
}