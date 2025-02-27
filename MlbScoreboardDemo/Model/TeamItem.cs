using System;
using System.Diagnostics;
using Windows.Data.Json;

namespace MlbScoreboardDemo.Model
{
    public class TeamItem
    {
        public string Name { get; set; }
        public string City { get; set; }
		public string Abbreviation { get; set; }

        public static TeamItem CreateWithJson(string json, bool homeTeam = true)
        {
            var teamItem = new TeamItem();
            var jsonObject = JsonObject.Parse(json);

			#region Name
			try
	        {
		        teamItem.Name = jsonObject[homeTeam ? "home_team_name" : "away_team_name"].ToString().Trim('\"');
	        }
	        catch (Exception e)
	        {
				Debug.Write(e);
		        teamItem.Name = "";
	        }
			#endregion

			#region City
			try
	        {
		        teamItem.City = jsonObject[homeTeam ? "home_team_city" : "away_team_city"].ToString().Trim('\"');
	        }
	        catch (Exception e)
	        {
		        Debug.WriteLine(e);
		        teamItem.City = "";
	        }
			#endregion

			#region Abbreviation
			try
			{
				teamItem.Abbreviation = jsonObject[homeTeam ? "home_name_abbrev" : "away_name_abbrev"].ToString().Trim('\"');
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				teamItem.Abbreviation = "";
			}
			#endregion


			return teamItem;
        }
    }
}
