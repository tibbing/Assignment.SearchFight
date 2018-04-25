using System;
using System.Collections.Generic;
using Assignment.SearchFight.Interfaces;
using Assignment.SearchFight.Models;
using Assignment.SearchFight.SearchProviders;

namespace Assignment.SearchFight
{
	class Program
	{
		static readonly List<ISearchProvider> SearchProviders = new List<ISearchProvider>()
		{
			new BingSearcher(),
			new GoogleSearcher()
		};

		static void Main(string[] args)
		{
			//args = new[] {"java", ".net"};
			var hitCounter = new HitCounter(SearchProviders);
			var results = hitCounter.SearchKeywords(args).Result;
			hitCounter.PrintResult(results);

			while (true)
			{
				Console.WriteLine("Enter new keywords..");
				var keywords = Console.ReadLine();
				results = hitCounter.SearchKeywords(keywords).Result;
				hitCounter.PrintResult(results);
			}
		}

	}
}
