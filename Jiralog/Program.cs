using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Jiralog
{
	class MainClass
	{
		const string maxResults = "1000";
		const string login = "nzuev";
		const string password = "nzuev";
		const string WorkFeed = @"http://localhost:8080/activity?maxResults=" + maxResults + "&streams=user+IS+" + login + "&os_authType=basic&title=undefined";

		public static void Main (string[] args)
		{
			string data = null;

			using (var client = new WebClient()) {
				client.Credentials = new NetworkCredential (login, password);
				Console.WriteLine ("loading...");
				data = client.DownloadString (WorkFeed);
			}

			Console.WriteLine ("parsing...");
			var xml = XDocument.Parse (data);
			Console.WriteLine ("processing...");

			var report = new XDocument (new XElement ("html", new XElement ("title", "Report")));
			var body = new XElement ("body");

			var root = xml.Root;
			var feedUpdated = root.AtomString ("updated");
			Console.WriteLine ("Feed updated: {0}", feedUpdated);
			body.Add(new XElement("h1", "Feed updated: " + feedUpdated));

			TimeSpan totalWork = TimeSpan.Zero;

			var entries = new XElement ("ul");
			foreach (var entry in root.Elements(Rss.Atom("entry"))) {
				var work = new Work (entry);

				if (work.Type == WorkType.Commented) {
					continue;
				}

				work.Print (Console.Out);
				entries.Add (new XElement ("li", work.Html ()));

				if (work.Type == WorkType.Logged) {
					totalWork = work.Log.AddTo (totalWork);
				}
			}
			body.Add (entries);

			Console.WriteLine ();
			Console.WriteLine ("========================");
			Console.WriteLine ("total: {0}h", totalWork.TotalHours);

			body.Add (new XElement ("h1", "Total work"));
			body.Add (string.Format ("{0} hours", totalWork.TotalHours));
			report.Root.Add (body);
			report.Save ("report.html");

			Console.WriteLine ("done");
		}
	}
}

