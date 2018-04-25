using System.Collections.Generic;

namespace Assignment.SearchFight.Models
{
	public struct KeywordResults
	{
		public string Keyword { get; }
		public Dictionary<string, long> Results { get; }

		public KeywordResults(string keyword, Dictionary<string, long> results)
		{
			Results = results;
			Keyword = keyword;
		}

	}
}