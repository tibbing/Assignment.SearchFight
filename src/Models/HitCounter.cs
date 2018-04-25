using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assignment.SearchFight.Extensions;
using Assignment.SearchFight.Interfaces;

namespace Assignment.SearchFight.Models
{
	public class HitCounter
	{
		private readonly List<ISearchProvider> searchProviders;

		public HitCounter(List<ISearchProvider> providers)
		{
			searchProviders = providers;
		}
		public HitCounter(ISearchProvider provider)
		{
			searchProviders = new List<ISearchProvider>() { provider };
		}

		public void PrintResult(List<KeywordResults> results)
		{
			if (results == null)
			{
				Console.WriteLine("No keywords provided \n");
				return;
			}

			var padding = results
				.Select(p => p.Keyword)
				.Max(p => p.Length)
				.ClampMin(10);

			Console.WriteLine("\t" + results.Select(p => p.Keyword.PadRight(padding)).ToDelimitedString("\t"));
			foreach (var provider in searchProviders)
			{
				Console.WriteLine(
					provider.Name.PadRight(7) + "\t" +
					results
						.Select(p => p.Results[provider.Name].ToString().PadRight(padding))
						.ToDelimitedString("\t")
				);
			}
			Console.WriteLine(
				"Total\t" + results
					.Select(p => p.Results["Total"].ToString().PadRight(padding))
					.ToDelimitedString("\t") + "\n\n"
			);

			var list = searchProviders
				.Select(p => p.Name)
				.ToList();
			list.Add("Total");

 			foreach (var provider in list)
			{
				var winner = results
					.Aggregate((i1, i2) => i1.Results[provider] > i2.Results[provider] ? i1 : i2)
					.Keyword;

				Console.WriteLine($"{provider} winner: {winner}");
			}

			Console.WriteLine();
		}

		public async Task<List<KeywordResults>> SearchKeywords(string keywords)
		{
			string[] parts;
			if (string.IsNullOrWhiteSpace(keywords))
			{
				parts = new string[0];
			}
			else
			{
				parts = Regex.Matches(keywords, @"[\""].+?[\""]|[^ ]+")
					.Select(m => m.Value.Replace("\"", ""))
					.ToArray();
			}
			return await SearchKeywords(parts);
		}

		public async Task<List<KeywordResults>> SearchKeywords(string[] keywords)
		{
			if (!keywords.Any())
			{
				return null;
			}
			return (await Task.WhenAll(
				keywords.Select(
					async p => new KeywordResults(p, await SearchKeyword(p))
				)
			)).ToList();
		}

		private async Task<Dictionary<string, long>> SearchKeyword(string keyword)
		{
			var result = (await Task.WhenAll(
				searchProviders.Select(
					async p => new KeyValuePair<string, long>(p.Name, await p.GetHitCount(keyword.Trim()))
				)
			)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			result.Add("Total", result.Values.Sum());
			return result;
		}
	}
}
