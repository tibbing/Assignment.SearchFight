using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.SearchFight.Extensions;
using Assignment.SearchFight.Interfaces;
using Assignment.SearchFight.SearchProviders;

namespace Assignment.SearchFight
{
	class Program
	{
		static List<ISearchProvider> SearchProviders = new List<ISearchProvider>()
		{
			new BingSearcher(),
			new GoogleSearcher()
		};

		static void Main(string[] args)
		{
			args = new[] {"java", ".net"};

			var results = SearchKeywords(args).Result;
			PrintResult(results);

			while (true)
			{
				Console.WriteLine("Enter new keywords..");
				var keywords = Console.ReadLine()?.Split(" ") ?? new string[0];
				results = SearchKeywords(keywords).Result;
				PrintResult(results);
			}
		}

		public static void PrintResult(Dictionary<string, Dictionary<string, long>> results)
		{
			if (results == null)
			{
				Console.WriteLine("No keywords provided \n");
				return;
			}

			var padding = results.Keys
				.Max(p => p.Length)
				.ClampMin(10);

			Console.WriteLine("\t" + results.Keys.Select(p => p.PadRight(padding)).ToDelimitedString("\t"));
			foreach (var provider in SearchProviders)
			{
				Console.WriteLine(
					provider.Name.PadRight(7) + "\t" + 
					results.Values
						.Select(p => p[provider.Name].ToString().PadRight(padding))
						.ToDelimitedString("\t")
				);
			}
			Console.WriteLine(
				"Total\t" + results.Values
					.Select(p => p["Total"].ToString().PadRight(padding))
					.ToDelimitedString("\t") + "\n"
			);
		}

		public static async Task<Dictionary<string, Dictionary<string, long>>> SearchKeywords(string[] keywords)
		{
			if (!keywords.Any())
			{
				return null;
			}
			return (await Task.WhenAll(
				keywords.Select(
					async p => new KeyValuePair<string, Dictionary<string, long>>(p, await SearchKeyword(p))
				)
			)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		public static async Task<Dictionary<string,long>> SearchKeyword(string keyword)
		{
			var result = (await Task.WhenAll(
				SearchProviders.Select(
					async p => new KeyValuePair<string, long>(p.Name, await p.GetHitCount(keyword))
				)
			)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			result.Add("Total",result.Values.Sum());
			return result;
		}
	}
}
