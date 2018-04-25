﻿using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Assignment.SearchFight.SearchProviders
{
	public class GoogleSearcher : SearchProviderBase
	{
		public GoogleSearcher()
		{
			BaseUrl = "https://www.google.com/search";
		}
		public override async Task<long> HitsOfKeyword(string keyword)
		{
			var nameValueCollection = new NameValueCollection
			{
				{"hl", "en"}, 
				{"q", keyword}
			};

			var html = await GetHtml(nameValueCollection);
			return TryParseHtml(html, @"About (.*) results\<\/div\>");
		}
	}
}
