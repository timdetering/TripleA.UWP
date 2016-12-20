using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using TripleA.Events;
using TripleA.Model;

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

        public MapViewModel()
        {

            InitializeEventHandlers();
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
        }
    }
}