using System;
using System.Xml.Linq;

namespace Jiralog
{
	public class Timestamp
	{
		public string Published { get; private set; }
		public string Updated { get; private set; }

		public Timestamp (XElement entry)
		{
			Published = entry.AtomString ("published");
			Updated = entry.AtomString ("updated");
		}

		public override string ToString ()
		{
			return Updated ?? Published ?? "unknown";
		}
	}

	/*
<entry>
	<published>2013-08-22T17:10:43.087Z</published>
    <updated>2013-08-22T17:10:43.087Z</updated>
</entry>

    */
}

