using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Views;

namespace TripleA.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MainViewModel()
        {
            this.MenuItems = new ObservableCollection<MenuItemViewModel>();

            this.InitializeOptions();
        }

        private void InitializeOptions()
        {
            this.MenuItems.Clear();
            var mapListItem = new MenuItemViewModel() { Label = "Map", SymbolAsChar = '\uE707', PageType = typeof(MapView) };

            this.MenuItems.Add(mapListItem);
        }
    }
}