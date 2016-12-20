using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using TripleA.Model;

namespace TripleA.ViewModel
{
    public class NewGameViewModel : ViewModelBase
    {
        private Scenario selectedScenario;

        public Scenario SelectedScenario
        {
            get { return selectedScenario; }
            set { selectedScenario = value; this.RaisePropertyChanged(); }
        }

        public ObservableCollection<Scenario> Scenarios { get; set; }

        public NewGameViewModel()
        {
            Scenarios = new ObservableCollection<Scenario>();

            Scenarios.Add(new Scenario() { Name = "Classic" });
            Scenarios.Add(new Scenario() { Name = "WaW" });

            this.InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
        }
    }
}