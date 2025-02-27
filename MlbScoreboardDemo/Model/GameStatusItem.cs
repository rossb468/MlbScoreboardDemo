using System;
using System.Diagnostics;
using Windows.Data.Json;

namespace MlbScoreboardDemo.Model
{
	public class GameStatusItem
	{
		public string Inning { get; set; }
		public bool TopInning { get; set; }
		public string Status { get; set; }

		public bool IsCompleted => Status.ToLower() == "final";
		public bool IsPostponed => Status.ToLower() == "postponed";

		public static GameStatusItem CreateWithJson(string json)
		{
			var statusItem = new GameStatusItem();
			var jsonObject = JsonObject.Parse(json);

			#region Inning
			try
			{
				statusItem.Inning = jsonObject["inning"].ToString().Trim('\"');
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				statusItem.Inning = "";
			}
			#endregion

			#region TopInning
			try
			{
				var topInningString = jsonObject["top_inning"].ToString().Trim('\"');
				statusItem.TopInning = topInningString.ToLower() == "y";
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				statusItem.TopInning = false;
			}
			#endregion

			#region Status
			try
			{
				statusItem.Status = jsonObject["status"].ToString().Trim('\"');
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				statusItem.Status = "";
			}
			#endregion

			return statusItem;
		}

		public GameStatusItem()
		{
			Inning = "";
			TopInning = false;
			Status = "";
		}
	}
}
