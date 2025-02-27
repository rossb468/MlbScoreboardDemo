using System;
using System.Diagnostics;
using Windows.Data.Json;

namespace MlbScoreboardDemo.Model
{
	public class PitcherItem
	{
		public string Saves { get; set; }
		public string Losses { get; set; }
		public string Era { get; set; }
		public string Number { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string Wins { get; set; }

		public string Description => $"{LastName} ({Wins}-{Losses})";
		public string SaveDescription => $"{LastName} ({Saves})";
		public bool IsEmpty =>  string.IsNullOrEmpty(Number)
							&& string.IsNullOrEmpty(LastName)
							&& string.IsNullOrEmpty(FirstName)
							&& Losses == "0"
							&& Wins == "0"
							&& Era == "0"
							&& Saves == "0";

		public static PitcherItem CreateWithJson(string json)
		{
			var pitcherItem = new PitcherItem();
			var jsonObject = JsonObject.Parse(json);

			#region ERA

			try
			{
				pitcherItem.Era = jsonObject["era"].ToString().Trim('\"');
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.Era = "";
			}

			#endregion

			#region LastName

			try
			{
				pitcherItem.LastName = jsonObject["last"].ToString().Trim('\"');
				if (pitcherItem.LastName == "")
				{
					pitcherItem.LastName = jsonObject["name_display_roster"].ToString().Trim('\"');
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.LastName = "";
			}

			#endregion

			#region Losses

			try
			{
				pitcherItem.Losses = jsonObject["losses"].ToString().Trim('\"');
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.Losses = "";
			}

			#endregion

			#region Number
			try { pitcherItem.Number = jsonObject["number"].ToString().Trim('\"'); }
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.Number = "";
			}
			#endregion

			#region FirstName
			try { pitcherItem.FirstName = jsonObject["first"].ToString().Trim('\"'); }
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.FirstName = "";
			}
			#endregion

			#region Wins
			try { pitcherItem.Wins = jsonObject["wins"].ToString().Trim('\"'); }
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.Wins = "";
			}
			#endregion

			#region Saves
			// This will often throw, because "saves" is not included for winning or losing pitches
			try { pitcherItem.Saves = jsonObject["saves"].ToString().Trim('\"'); }
			catch (Exception e)
			{
				Debug.WriteLine(e);
				pitcherItem.Saves = "";
			}
			#endregion

			return pitcherItem;
		}

		public PitcherItem()
		{
			Saves = "";
			Losses = "";
			Era = "";
			Number = "";
			LastName = "";
			FirstName = "";
			Wins = "";
		}
	}
}
