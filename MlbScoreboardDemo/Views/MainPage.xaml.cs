using System;
using System.Linq;
using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MlbScoreboardDemo.BusinessLogic;
using MlbScoreboardDemo.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MlbScoreboardDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
	    public MainPage()
	    {
		    this.InitializeComponent();
		}

	    protected override void OnNavigatedTo(NavigationEventArgs e)
	    {
		    base.OnNavigatedTo(e);
			var vm = (MainVM)DataContext;
		    vm.SelectedDate = DateTime.Today;
			vm.Initialize();
		}

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addedItem = e.AddedItems.FirstOrDefault() as GameItem;
            var removedItem = e.RemovedItems.FirstOrDefault() as GameItem;

            if (addedItem != null) { addedItem.IsSelected = true; }
            if (removedItem != null) { removedItem.IsSelected = false; }
        }

	    private void PreviousDateButton_OnClick(object sender, RoutedEventArgs e)
	    {
			var vm = (MainVM)DataContext;
			vm.ChangeDateBackward();
		}

		private void NextDateButton_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = (MainVM)DataContext;
			vm.ChangeDateForward();
		}
	}
}
