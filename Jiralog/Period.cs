using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jiralog
{
	/// <summary>
	/// 1 year, 47 weeks, 2 days, 6 hours, 50 minutes
	/// </summary>
	public class Period
	{
		static readonly Regex _years = new Regex(@"(\d+) years?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		// months?
		static readonly Regex _weeks = new Regex(@"(\d+) weeks?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		static readonly Regex _days = new Regex(@"(\d+) days?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		static readonly Regex _hours = new Regex(@"(\d+) hours?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		static readonly Regex _minutes = new Regex(@"(\d+) minutes?", RegexOptions.IgnoreCase | RegexOptions.Compiled);


		public string RawData { get; private set; }
		public TimeSpan Span { get; private set; }

		public Period (string rawData)
		{
			RawData = rawData;
			Span = TimeSpan.Zero;

			foreach (var part in rawData.Split(',')) {
				var trimmed = part.Trim();

				var match = _minutes.Match (trimmed);
				if (match.Success) {
					Span = Span.Add (TimeSpan.FromMinutes (TakeNumber (match)));
					continue;
				}

				match = _hours.Match (trimmed);
				if (match.Success) {
					Span = Span.Add (TimeSpan.FromHours (TakeNumber (match)));
					continue;
				}

				match = _days.Match (trimmed);
				if (match.Success) {
					Span = Span.Add (TimeSpan.FromDays (TakeNumber (match)));
					continue;
				}

				match = _weeks.Match (trimmed);
				if (match.Success) {
					Span = Span.Add (TimeSpan.FromDays (7 * TakeNumber (match)));
					continue;
				}

				match = _years.Match (trimmed);
				if (match.Success) {
					Span = Span.Add (TimeSpan.FromDays (365 * TakeNumber (match)));
					continue;
				}

				throw new NotSupportedException("Unsupported period: " + rawData);
			}
		}

		int TakeNumber(Match match)
		{
			return int.Parse ((string)match.Groups [1].Value);
		}

		public TimeSpan AddTo(TimeSpan span)
		{
			return span.Add (Span);
		}

		public override string ToString ()
		{
			return RawData ?? "unknown period";
		}
	}
}

