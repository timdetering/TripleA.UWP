using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TripleA.Events;
using TripleA.Model;
using TripleA.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TripleA.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {
        MapViewModel viewModel;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button territoryButton = (Button)sender;

            var clickedTerritory = territoryButton.DataContext as Territory;
            viewModel.Map.SelectedTerritory = clickedTerritory;
        }

        private void Canvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.MapCanvas.Opacity = 1.0;
        }

        private void Canvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.MapTransform.TranslateX += e.Delta.Translation.X;
            this.MapTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void Canvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.MapCanvas.Opacity = 1.0;
        }
    }
}
