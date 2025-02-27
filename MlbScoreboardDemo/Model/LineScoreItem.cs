using System;
using System.Diagnostics;
using Windows.Data.Json;

namespace MlbScoreboardDemo.Model
{
	public class LineScoreItem
	{
		public string HomeTeamRuns { get; set; }
		public string AwayTeamRuns { get; set; }

		public static LineScoreItem CreateWithJson(string json)
		{
			var lineScoreItem = new LineScoreItem();
			var jsonObject = JsonObject.Parse(json);

			try
			{
				var runsObject = JsonObject.Parse(jsonObject["r"].ToString());

				#region Home
				try
				{
					lineScoreItem.HomeTeamRuns = runsObject["home"].ToString().Trim('\"');
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
					lineScoreItem.HomeTeamRuns = "";
				}
				#endregion

				#region Away
				try
				{
					lineScoreItem.AwayTeamRuns = runsObject["away"].ToString().Trim('\"');
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
					lineScoreItem.AwayTeamRuns = "";
				}
				#endregion
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}

			return lineScoreItem;
		}

		public LineScoreItem()
		{
			HomeTeamRuns = "";
			AwayTeamRuns = "";
		}
	}
}
