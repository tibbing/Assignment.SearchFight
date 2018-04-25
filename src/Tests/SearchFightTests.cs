using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.SearchFight.Interfaces;
using Assignment.SearchFight.Models;
using Assignment.SearchFight.SearchProviders;
using NUnit.Framework;


namespace Tests
{
	[TestFixture]
	public class SearchFightTests
	{
		[Test]
		public async Task ShouldGetSearchResultsFromGoogle()
		{
			var googleSearcher = new GoogleSearcher();
			var hitCounter = new HitCounter(googleSearcher);
			
			var results = await hitCounter.SearchKeywords("test");
			
			Assert.That(results.Count == 1,"Expected keywords in results to be one");
			Assert.That(results.Any(p => p.Keyword == "test"),"Keyword does not exist in results");
			var keywordResults = results.First().Results;
			Assert.That(keywordResults.Any(p => p.Key == googleSearcher.Name), "No results from google");
			Assert.That(keywordResults.Any(p => p.Value > 0), "Expected more than 0 results");
		}
		
		[Test]
		public async Task ShouldGetSearchResultsFromBing()
		{
			var bingSearcher = new BingSearcher();
			var hitCounter = new HitCounter(bingSearcher);
			
			var results = await hitCounter.SearchKeywords("test");
			
			Assert.That(results.Count == 1,"Expected keywords in results to be one");
			Assert.That(results.Any(p => p.Keyword == "test"),"Keyword does not exist in results");
			var keywordResults = results.First().Results;
			Assert.That(keywordResults.Any(p => p.Key == bingSearcher.Name), "No results from bing");
			Assert.That(keywordResults.Any(p => p.Value > 0), "Expected more than 0 results");
		}

		[Test]
		public async Task ShouldIncludeTotalResults()
		{
			var googleSearcher = new GoogleSearcher();
			var bingSearcher = new BingSearcher();
			var hitCounter = new HitCounter(new List<ISearchProvider>() { googleSearcher,bingSearcher });
			
			var results = await hitCounter.SearchKeywords("test");

			var keywordResults = results.First().Results;
			Assert.That(keywordResults.Any(p => p.Key == "Total"), "No value for total hits");

			var numTotal = keywordResults.First(p => p.Key == "Total").Value;
			var numProvidersSum = keywordResults.Where(p => p.Key != "Total").Sum(p => p.Value);
			Assert.That(numTotal == numProvidersSum, "Total value does not equal providers total value");
		}
		
		[Test]
		public async Task ShouldHandleQuotedKeywords()
		{
			var googleSearcher = new GoogleSearcher();
			var hitCounter = new HitCounter(googleSearcher);
			
			var results = await hitCounter.SearchKeywords(@""".net"" ""java""");

			Assert.That(results.Count == 2,"Expected keywords in results to be 2");
			Assert.That(results.Any(p => p.Keyword == ".net"),"Expected keyword .net");
			Assert.That(results.Any(p => p.Keyword == "java"),"Expected keyword java");
		}
		
	}
}