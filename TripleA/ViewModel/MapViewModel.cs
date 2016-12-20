using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TripleA.Events;
using TripleA.Model;
using System;
using System.Linq;
using System.Diagnostics;

namespace TripleA.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        public Map Map
        {
            get
            {
                return Game.Instance.Map;
            }
        }

        public ObservableCollection<Unit> UnitsToMove { get; set; }

        private bool isMoveInProgress;

        public bool IsMoveInProgress
        {
            get { return isMoveInProgress; }
            set
            {
                isMoveInProgress = value;
                if (!value)
                {
                    this.CancelMove();
                }
                this.RaisePropertyChanged();
            }
        }
        private bool isTerritorySelected;
        public bool IsTerritorySelected
        {
            get { return isTerritorySelected; }
            set
            {
                isTerritorySelected = value;
                RaisePropertyChanged();
                if(!IsTerritorySelected)
                {
                    this.Map.SelectedTerritory = null;
                }
            }
        }

        private void CancelMove()
        {
            this.UnitsToMove.Clear();
        }

        public MapViewModel()
        {
            InitializeEventHandlers();

            this.UnitsToMove = new ObservableCollection<Unit>();
        }

        private void InitializeEventHandlers()
        {
            Messenger.Default.Register<GameInitializationCompleted>(this, HandleGameInitializationCompletedEvent);
        }

        private void HandleGameInitializationCompletedEvent(GameInitializationCompleted obj)
        {
            this.RaisePropertyChanged("Map");
        }

        public async Task Initialize()
        {
            Messenger.Default.Register<TerritorySelectionChanged>(this, OnTerritorySelectionChanged);
        }

        private void OnTerritorySelectionChanged(TerritorySelectionChanged territorySelection)
        {
            Debug.WriteLine("Territory Selection Changed");
            this.IsTerritorySelected = territorySelection != null;
        }

        public void AddUnitsToMove(Unit clickedUnit)
        {
            IsMoveInProgress = true;
            var unitToMove = clickedUnit.Clone();

            var existingUnit = (from x in this.UnitsToMove
                               where x.Type == unitToMove.Type &&
                               x.Owner == unitToMove.Owner
                               select x).FirstOrDefault();

            if(existingUnit != null)
            {
                existingUnit.Quantity += unitToMove.Quantity;
            }
            else
            {
                this.UnitsToMove.Add(unitToMove);
            }
        }
    }
}