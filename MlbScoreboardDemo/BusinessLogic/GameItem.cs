using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Windows.Globalization.DateTimeFormatting;
using MlbScoreboardDemo.Annotations;
using MlbScoreboardDemo.Model;

namespace MlbScoreboardDemo.BusinessLogic
{
    public class GameItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public TeamItem HomeTeam { get; set; }
        public TeamItem AwayTeam { get; set; }
        public DateTime GameDate { get; set; }
        public CachedImageHelper ThumbnailImage { get; set; }

		public PitcherItem WinningPitcher { get; set; }
		public PitcherItem LosingPitcher { get; set; }
		public PitcherItem SavePitcher { get; set; }

		public LineScoreItem LineScore { get; set; }
		public GameStatusItem StatusItem { get; set; }

	    public bool GameHasStarted => GameDate < DateTime.Now;
		public string GameTimeZone { get; set; }

	    public static GameItem CreateWithJson(string json)
        {
            var gameItem = new GameItem();
            var jsonObject = JsonObject.Parse(json);
            gameItem.HomeTeam = TeamItem.CreateWithJson(json, true);
            gameItem.AwayTeam = TeamItem.CreateWithJson(json, false);

			#region Line Score
	        try
	        {
		        var lineScoreJson = jsonObject["linescore"].ToString();
				gameItem.LineScore = LineScoreItem.CreateWithJson(lineScoreJson); 
	        }
	        catch (Exception e)
	        {
		        Debug.WriteLine(e);
				gameItem.LineScore = new LineScoreItem();
	        }
			#endregion

			#region Winning Pitcher
			try
	        {
		        string winningPitcherJson = jsonObject["winning_pitcher"].ToString().Trim('\"');
		        gameItem.WinningPitcher = PitcherItem.CreateWithJson(winningPitcherJson);
	        }
	        catch (Exception e)
	        {
				Debug.WriteLine("Error parsing winning pitcher");
				Debug.WriteLine(e);
		        gameItem.WinningPitcher = new PitcherItem();
	        }
			#endregion

			#region Losing Pitcher
			try
			{
				string losingPitcherJson = jsonObject["losing_pitcher"].ToString().Trim('\"');
				gameItem.LosingPitcher = PitcherItem.CreateWithJson(losingPitcherJson);
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error parsing losing pitcher");
				Debug.WriteLine(e);
				gameItem.LosingPitcher = new PitcherItem();
			}
			#endregion

			#region Save Pitcher
			try
			{
				string savePitcherJson = jsonObject["save_pitcher"].ToString().Trim('\"');
				gameItem.SavePitcher = PitcherItem.CreateWithJson(savePitcherJson);
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error parsing save pitcher");
				Debug.WriteLine(e);
				gameItem.SavePitcher = new PitcherItem();
			}
			#endregion

			#region Game Date

	        try
	        {
				// TODO: Make this handle rescheduled games
		        var gameDateString = jsonObject["original_date"].ToString().Trim('\"');
				var gameTimeString = jsonObject["home_time"].ToString().Trim('\"');
				var ampm = jsonObject["home_ampm"].ToString().Trim('\"');
				var homeTimeZone = jsonObject["home_time_zone"].ToString().Trim('\"');

				var gameDateTime = Convert.ToDateTime($"{gameTimeString} {gameDateString} {ampm}");

		        switch (homeTimeZone)
		        {
			        case "ET":
				        gameDateTime = gameDateTime.AddHours(4);
				        break;
			        case "CT":
						gameDateTime = gameDateTime.AddHours(5);
				        break;
			        case "MT":
						gameDateTime = gameDateTime.AddHours(6);
				        break;
			        case "PT":
						gameDateTime = gameDateTime.AddHours(7);
				        break;
		        }

		        gameItem.GameDate = TimeZoneInfo.ConvertTime(gameDateTime, TimeZoneInfo.Utc, TimeZoneInfo.Local);
		        gameItem.GameTimeZone = homeTimeZone;
	        }
			catch (FormatException e)
			{
				Debug.WriteLine("Error converting date");
				throw e;

			}
			catch (Exception e)
	        {
		        Debug.WriteLine(e);
		        gameItem.GameDate = DateTime.Today;
		        // TODO: Make format match json format
	        }
			#endregion

			#region Thumnail Image
			try
	        {
		        gameItem.ThumbnailImage = new CachedImageHelper(jsonObject["video_thumbnail"].ToString().Trim('\"'));
	        }
	        catch (Exception e)
	        {
				Debug.WriteLine(e);
				gameItem.ThumbnailImage = new CachedImageHelper();
	        }
			#endregion

			#region Status
			try
			{
				gameItem.StatusItem = GameStatusItem.CreateWithJson(jsonObject["status"].ToString());
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				gameItem.StatusItem = new GameStatusItem();
			}
			#endregion

			return gameItem;
        }

	    private DateTime ConvertDateToLocalTime(DateTime gameDate)
	    {
		    var gameTimeZone = GameTimeZone;
		    var localTimeZone = TimeZoneInfo.Local;
		    var offset = 0;
		    if (gameTimeZone == "ET")
		    {
			    if (localTimeZone.Id == "Pacific Standard Time")
			    {
				    offset = -3;
			    }
			    if (localTimeZone.Id == "Mountain Standard Time")
			    {
				    offset = -2;
			    }
			    if (localTimeZone.Id == "Central Standard Time")
			    {
				    offset = -1;
			    }
		    }

		    return DateTime.Now;
	    }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
