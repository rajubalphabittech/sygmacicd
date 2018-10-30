using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Public
/// </summary>
namespace Search {
	public enum SearchType {
		AllWords,
		AnyWords,
		ExactPhrase
	}
	public enum SortBy { 
		Relevance,
		Date
	}
	public enum Source { 
		Sygmanet,
		EROFileServer,
		WROFileServer
	}
	public class Common {
		public static string FormatSearchText(string text, string searchType) {
			if (searchType == null)
				searchType = "";
			if (searchType != "")
				return FormatSearchText(text, (SearchType)Enum.Parse(typeof(SearchType), searchType, true));
			else
				return FormatSearchText(text);
		}
		public static string FormatSearchText(string text) {
			return FormatSearchText(text, SearchType.AllWords);
		}
		public static string FormatSearchText(string text, SearchType searchType) {
			string retVal = "";
			text = text.Trim();
			while (text.IndexOf("  ") != -1) {
				text = text.Replace("  ", " ");
			}
			text = text.Replace(" ", "+");
			switch (searchType) {
				case SearchType.AllWords:
					retVal = text.Replace("+", "+AND+");
					break;
				case SearchType.ExactPhrase:
					retVal = string.Format("\"{0}\"", text);
					break;
				default:
					retVal = text;
					break;
			}
			return retVal;
		}
	}
}