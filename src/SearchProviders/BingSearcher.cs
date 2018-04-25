using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Assignment.SearchFight.SearchProviders
{
	public class BingSearcher : SearchProviderBase
	{
		public override string Name => "Bing";
		protected override string BaseUrl => "https://www.bing.com/search";
		public override async Task<long> GetHitCount(string keyword)
		{
			var nameValueCollection = new NameValueCollection
			{
				{"setlang", "en-us"},
				{"q", keyword}
			};

			var html = await GetHtml(nameValueCollection);
			return TryParseHtml(html, @"\<span class=\""sb_count\"">(.*) results\<\/span\>");
		}

	}
}
