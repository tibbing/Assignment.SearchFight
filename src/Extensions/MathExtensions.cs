using System;

namespace Assignment.SearchFight.Extensions
{
	public static class MathExtensions
	{
		public static T ClampMin<T>(this T val, T min) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			return val;
		}

		public static T ClampMax<T>(this T val, T max) where T : IComparable<T>
		{
			if (val.CompareTo(max) < 0)
			{
				return max;
			}
			return val;
		}
	}
}