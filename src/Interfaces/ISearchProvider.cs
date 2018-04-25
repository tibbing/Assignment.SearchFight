using System.Threading.Tasks;

namespace Assignment.SearchFight.Interfaces
{
	public interface ISearchProvider
	{
		Task<long> HitsOfKeyword(string keyword);
	}
}