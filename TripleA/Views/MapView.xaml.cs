using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using TripleA.Events;
using TripleA.Model;
using TripleA.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace TripleA.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {
        MapViewModel viewModel;

        // Keeping the Drag operation will allow to cancel it after it has started
        IAsyncOperation<DataPackageOperation> _dragOperation;
        bool isInitialized = false;

        public MapView()
        {
            this.InitializeComponent();

            viewModel = new MapViewModel();
            this.DataContext = viewModel;

            this.Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Map" });
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
            {

                await viewModel.Initialize();

                MapViewer.ChangeView(0, 0, 0.4f);
                isInitialized = true;
            }
        }

        private void UnitClicked(object sender, RoutedEventArgs e)
        {
            Button unitButton = (Button)sender;

            var clickedUnit = unitButton.DataContext as Unit;

            MapViewer.ManipulationMode = ManipulationModes.System;

            viewModel.AddUnitsToMove(clickedUnit);
        }

        private void TerritoryClicked(object sender, RoutedEventArgs e)
        {
            Button territoryButton = (Button)sender;

            var clickedTerritory = territoryButton.DataContext as Territory;
            viewModel.Map.SelectedTerritory = clickedTerritory;
        }

        private void Canvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationStarted");
            e.Handled = false;
            this.MapCanvas.Opacity = 1.0;
        }

        private void Canvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationDelta");
            e.Handled = false;
            this.MapTransform.TranslateX += e.Delta.Translation.X;
            this.MapTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void Canvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationCompleted");
            e.Handled = false;
            this.MapCanvas.Opacity = 1.0;
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Pointer Pressed over unit");
        }

        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Pointer Released over unit");
        }

        private void Button_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Pointer Moved");
        }
    }
}