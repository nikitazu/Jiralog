using System;
using System.Xml.Linq;

namespace Jiralog
{
	public class Author
	{
		public string Name { get; private set; }
		public string Email { get; private set; }

		public Author (XElement entry)
		{
			var author = entry.AtomElement("author");
			Name = author.AtomString ("name");
			Email = author.AtomString ("email");
		}

		public override string ToString ()
		{
			return Name ?? Email ?? "unknown";
		}
	}


	/*

<entry>
    <author>
      <name>Nikita Zuev</name>
      <email>nikitazu@gmail.com</email>
    </author>
</entry>

	 */
}

