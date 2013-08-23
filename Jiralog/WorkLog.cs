using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Jiralog
{
	public class WorkLog
	{
		public List<LogEntry> Items { get; private set; }

		public WorkLog (XElement entry)
		{
			Items = new List<LogEntry> ();

			var data = entry.AtomString ("content");
			if (string.IsNullOrWhiteSpace (data)) {
				return;
			}

			Items.AddRange (data
				.Split ('\n')
				.Where (line => line.Contains ("<li>"))
			    .Select (line => line.Replace ("<li>", "").Trim ())
			    .Select (line => new LogEntry (line)));
		}

		public TimeSpan AddTo(TimeSpan span)
		{
			var total = span;
			foreach (var item in Items) {
				total = item.AddTo (span);
			}
			return total;
		}
	}


	/*
<entry>
    <content type="html">&lt;ul class="updates activity-list"&gt;
  &lt;li&gt;Changed the Remaining Estimate to '1 year, 47 weeks, 4 days, 4 hours, 50 minutes'
  &lt;li&gt;Logged '2 hours'
&lt;/ul&gt;
</content>
</entry>


<ul class="updates activity-list">
  <li>Changed the Remaining Estimate to '1 year, 47 weeks, 4 days, 4 hours, 50 minutes'
  <li>Logged '2 hours'
</ul>

	 */
}

