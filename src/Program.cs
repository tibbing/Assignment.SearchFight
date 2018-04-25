using System;
using System.Threading.Tasks;
using Assignment.SearchFight.SearchProviders;

namespace Assignment.SearchFight
{
	class Program
	{
		static void Main(string[] args)
		{
			Search("java").Wait();
			Console.ReadKey();
		}

		public static async Task Search(string keyword)
		{
			var google = new GoogleSearcher();
			var bing = new BingSearcher();
			var numHitsBing = await bing.HitsOfKeyword(keyword);
			var numHitsGoogle = await google.HitsOfKeyword(keyword);
		}
	}
}
