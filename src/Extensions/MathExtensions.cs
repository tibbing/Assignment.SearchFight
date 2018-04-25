using System;

namespace Assignment.SearchFight.Extensions
{
	public static class MathExtensions
	{
		public static T ClampMin<T>(this T val, T min) where T : IComparable<T>
		{
			return val.CompareTo(min) < 0 ? min : val;
		}

		public static T ClampMax<T>(this T val, T max) where T : IComparable<T>
		{
			return val.CompareTo(max) < 0 ? max : val;
		}
	}
}