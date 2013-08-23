using System;
using System.Xml.Linq;

namespace Jiralog
{
	public static class Rss
	{
		static readonly XNamespace _atom = "http://www.w3.org/2005/Atom";
		static readonly XNamespace _atlassian = "http://streams.atlassian.com/syndication/general/1.0";
		static readonly XNamespace _activity = "http://activitystrea.ms/spec/1.0/";

		public static XName Atom(string name)
		{
			return _atom + name;
		}

		public static XName Atlassian(string name)
		{
			return _atlassian + name;
		}

		public static XName Activity(string name)
		{
			return _activity + name;
		}

		public static bool TryElement(this XElement element, ref XElement target, XName name)
		{
			if (element == null) {
				return false;
			}

			if (name == null) {
				return false;
			}

			var elt = element.Element (name);
			if (elt == null) {
				return false;
			}

			target = elt;
			return true;
		}

		public static bool TryAtomElement(this XElement element, ref XElement target, string name)
		{
			return element.TryElement (ref target, Atom (name));
		}

		public static string AtomString(this XElement element, string name)
		{
			XElement target = null;
			return element.TryAtomElement (ref target, name) ? target.Value : null;
		}

		public static XElement AtomElement(this XElement element, string name)
		{
			return element == null ? null : element.Element (Rss.Atom (name));
		}

		public static string ActivityString(this XElement element, string name)
		{
			var activ = element.ActivityElement (name);
			return activ == null ? null : activ.Value;
		}

		public static XElement ActivityElement(this XElement element, string name)
		{
			return element == null ? null : element.Element (Rss.Activity (name));
		}

		public static string AttributeValue(this XElement element, string name)
		{
			if (element == null) {
				return null;
			}

			var attr = element.Attribute (name);
			return attr == null ? null : attr.Value;
		}
	}
}

