using GalaSoft.MvvmLight.Messaging;
using TripleA.Events;
using TripleA.Model;
using TripleA.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TripleA.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGameView : Page
    {
        NewGameViewModel viewModel;

        public NewGameView()
        {
            this.InitializeComponent();

            viewModel = new NewGameViewModel();

            this.DataContext = viewModel;


            Messenger.Default.Register<GameInitializationCompleted>(this, HandleGameInitializationCompletedEvent);
        }

        private void HandleGameInitializationCompletedEvent(GameInitializationCompleted obj)
        {
            this.Frame.Navigate(typeof(MapView));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "New Game" });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedScenario != null)
            {
                var scenarioName = viewModel.SelectedScenario.Name;
                var variationName = "World_At_War" + ".xml";

                await Game.Instance.Initialize(scenarioName, variationName);

                //await Game.Instance.Initialize("Classic", "classic.xml");
                //await Game.Instance.Initialize("WaW", "World_At_War.xml");
            }
        }
    }
}