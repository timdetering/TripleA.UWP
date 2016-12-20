using System;
using TripleA.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TripleA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();

            viewModel = new MainViewModel();
            this.DataContext = viewModel;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ContentFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }
        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MenuItemViewModel;
            if (item.PageType != null)
            {
                ContentFrame.Navigate(item.PageType);
            }
        }
        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MenuItemViewModel;
            if (item.PageType != null)
            {
                ContentFrame.Navigate(item.PageType);
            }
        }
    }
}