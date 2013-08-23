using System;

namespace Jiralog
{
	/// <summary>
	/// Changed the Remaining Estimate to '1 year, 47 weeks, 2 days, 6 hours, 50 minutes'
	/// Logged '1 day, 3 hours'
	/// </summary>
	public class LogEntry
	{
		public string RawData { get; private set; }
		public string Title { get; private set; }
		public Period Period { get; private set; }

		public LogEntry (string rawData)
		{
			RawData = rawData;

			if (rawData.StartsWith ("Logged ")) {
				Title = "Logged";
				Period = new Period (rawData.Replace ("Logged ", string.Empty).Replace("'", string.Empty));
				return;
			}

			if (rawData.StartsWith ("Changed the Remaining Estimate to ")) {
				Title = "Reestimated";
				Period = new Period (rawData.Replace ("Changed the Remaining Estimate to ", string.Empty).Replace("'", string.Empty));
				return;
			}
		}

		public TimeSpan AddTo(TimeSpan span)
		{
			return Title == "Logged" ? Period.AddTo (span) : span;
		}

		public override string ToString ()
		{
			return RawData ?? "unknown entry";
		}
	}
}

