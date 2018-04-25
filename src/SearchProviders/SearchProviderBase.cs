using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assignment.SearchFight.Interfaces;

namespace Assignment.SearchFight.SearchProviders
{
	public abstract class SearchProviderBase : ISearchProvider
	{
		public abstract Task<long> GetHitCount(string keyword);
		public abstract string Name { get; }
		protected string BaseUrl { get; set; }

		protected long TryParseHtml(string html, string pattern)
		{
			var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
			if (!matches.Any())
			{
				return 0;
				//throw new Exception($"Invalid result data returned (no matches for pattern '{pattern}')");
			}

			var resultGroup = matches.First().Groups[1];
			var decoded = WebUtility.HtmlDecode(resultGroup.ToString());
			var numStr = Regex.Replace(decoded, "[^0-9]", "");

			var found = long.TryParse(numStr, out var num);
			if (!found)
			{
				throw new Exception($"Failed to parse data: {numStr}");
			}

			return num;
		}

		protected async Task<string> GetHtml(NameValueCollection queryStringCollection)
		{
			if (string.IsNullOrWhiteSpace(BaseUrl))
			{
				throw new Exception($"{nameof(BaseUrl)} has not been set");
			}
			using (var webClient = new WebClient())
			{
				webClient.Headers.Add(HttpRequestHeader.UserAgent, "Other");
				webClient.Headers.Add(HttpRequestHeader.Accept, "text/html");
				webClient.QueryString.Add(queryStringCollection);
				return await webClient.DownloadStringTaskAsync(BaseUrl);
			}
		}
	}
}