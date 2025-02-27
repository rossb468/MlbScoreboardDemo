using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;
using MlbScoreboardDemo.Annotations;
using MlbScoreboardDemo.BusinessLogic;

namespace MlbScoreboardDemo.ViewModels
{
	public class MainVM : INotifyCollectionChanged, INotifyPropertyChanged
	{

		private bool _loadInProgress;

		public bool LoadInProgress
		{
			get { return _loadInProgress; }
			set
			{
				_loadInProgress = value;
				OnPropertyChanged();
			}
		}

	    private GameItem _selectedItem;

	    public GameItem SelectedItem
	    {
	        get { return _selectedItem; }
	        set
	        {
	            _selectedItem = value;
	            OnPropertyChanged();
	        }
	    }

		private DateTime _selectedDate;

		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set
			{
				_selectedDate = value; 
				OnPropertyChanged();
			}
		}

	    public ObservableCollection<GameItem> GameList { get; set; }

	    public MainVM()
	    {
	        GameList = new ObservableCollection<GameItem>();
	    }

	    public async void Initialize()
	    {
		    LoadInProgress = true;
	        try
	        {
	            var gameList = await ApiClient.GetGameListForDate(SelectedDate);
	            if (gameList == null) return;

	            foreach (var game in gameList)
	            {
	                GameList.Add(GameItem.CreateWithJson(game.ToString()));
	            }
	        }
	        catch (Exception e)
	        {
	            Debug.WriteLine(e);
	        }
		    LoadInProgress = false;
	    }

		public async void ChangeDateBackward()
		{
			SelectedDate = SelectedDate.Subtract(TimeSpan.FromDays(1));
			GameList.Clear();
			Initialize();
		}

		public async void ChangeDateForward()
		{
			SelectedDate = SelectedDate.AddDays(1);
			GameList.Clear();
			Initialize();
		}

	    public event NotifyCollectionChangedEventHandler CollectionChanged;
	    public event PropertyChangedEventHandler PropertyChanged;

	    [NotifyPropertyChangedInvocator]
	    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
	        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
	}
}
