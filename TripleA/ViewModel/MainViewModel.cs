using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using TripleA.Events;
using TripleA.Views;

namespace TripleA.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; }
        public ObservableCollection<MenuItemViewModel> Options { get; private set; }

        public MainViewModel()
        {
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();
            this.Options = new ObservableCollection<MenuItemViewModel>();

            this.InitializeMenu();
            this.InitializeOptions();
            this.InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            Messenger.Default.Register<PageTitleChangedEvent>(this, HandlePageTitleChangedEvent);
        }

        private void HandlePageTitleChangedEvent(PageTitleChangedEvent payload)
        {
            this.Title = payload.Title;
        }

        private void InitializeMenu()
        {
            this.MenuItems.Clear();
            var mapListItem = new MenuItemViewModel() { Label = "Map", SymbolAsChar = '\uE707', PageType = typeof(MapView) };

            this.MenuItems.Add(mapListItem);
        }
        private void InitializeOptions()
        {
            this.Options.Clear();
            var mapListItem = new MenuItemViewModel() { Label = "New Game", SymbolAsChar = '\uE710', PageType = typeof(NewGameView) };

            this.Options.Add(mapListItem);
        }
    }
}