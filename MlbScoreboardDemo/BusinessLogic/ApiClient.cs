using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MlbScoreboardDemo.BusinessLogic
{
    public class ApiClient
    {
	    public static async Task<JArray> GetGameListForDate(DateTime date)
	    {
		    var urlString = buildUrlStringFromDate(date);

			var client = new HttpClient();
			var rawJsonString = await client.GetStringAsync(urlString);
			JObject parsedJson = JObject.Parse(rawJsonString);
			try
			{
				var gameArray = parsedJson["data"]["games"]["game"] as JArray;
				return gameArray;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return null;
			}
		}

	    private static string buildUrlStringFromDate(DateTime date)
	    {
		    var year = date.Year.ToString();
		    var month = date.Month.ToString("d2");
		    var day = date.Day.ToString("d2");

			var returnValue = $"http://gdx.mlb.com/components/game/mlb/year_{year}/month_{month}/day_{day}/master_scoreboard.json";

		    return returnValue;
	    }


    }
}
