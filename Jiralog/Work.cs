using System;
using System.Xml.Linq;
using System.IO;

namespace Jiralog
{
	public enum WorkType
	{
		Created,
		Started,
		Logged,
		Commented,
		Unknown
	}

	public class Work
	{
		public WorkType Type { get; private set; }
		public Author Author { get; private set; }
		public Timestamp Time { get; private set; }
		public WorkItem Item { get; private set; }
		public WorkLog Log { get; private set; }

		public Work (XElement entry)
		{
			Author = new Author (entry);
			Time = new Timestamp (entry);
			Type = ParseType (entry);
			Item = new WorkItem (entry);
			Log = new WorkLog (entry);
		}

		WorkType ParseType(XElement entry)
		{
			var term = entry.AtomElement ("category").AttributeValue ("term");
			var verb = entry.ActivityString ("verb");

			if (term == null && verb == "http://activitystrea.ms/schema/1.0/update") {
				return WorkType.Logged;
			}

			switch (term) {
				case "created": return WorkType.Created;
				case "started": return WorkType.Started;
				case "comment": return WorkType.Commented;
				default: return WorkType.Unknown;
			}
		}

		public void Print(TextWriter writer)
		{
			writer.WriteLine ("{0} - {1} {2}: {3}", Time, Author, Type, Item);
			foreach (var item in Log.Items) {
				writer.WriteLine ("  - {0}", item);
			}
		}

		public XElement Html()
		{
			var html = new XElement ("div");
			html.Add (new XElement ("p", string.Format ("{0} - {1} {2}: {3}", Time, Author, Type, Item)));
			var logs = new XElement ("ul");
			html.Add (logs);
			foreach (var item in Log.Items) {
				logs.Add (new XElement ("li", item.ToString ()));
			}
			return html;
		}
	}
}

