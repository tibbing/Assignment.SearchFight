using System.Threading.Tasks;

namespace Assignment.SearchFight.Interfaces
{
	public interface ISearchProvider
	{
		Task<long> GetHitCount(string keyword);
		string Name { get; }
	}
}