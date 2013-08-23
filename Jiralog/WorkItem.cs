using System;
using System.Xml.Linq;

namespace Jiralog
{
	public class WorkItem
	{
		public string Parent { get; private set; }
		public string Issue { get; private set; }

		public WorkItem (XElement entry)
		{
			var activity = entry.ActivityElement ("object");
			Parent = activity.AtomString ("title");
			Issue = activity.AtomString ("summary");
		}

		public override string ToString ()
		{
			return string.Format ("[{0}/{1}]", Parent ?? "_", Issue ?? "_");
		}
	}

	/*
<entry>
  <activity:object>
    <title>TEST-1</title>
    <summary>doit</summary>
  </activity:object>
</entry>
	 */
}

