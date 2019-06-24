using System.Collections.Generic;

namespace Left4Dots.Extension
{
	public static class ListExtension
	{
		public static void AddUnique<T>(this List<T> list, T item) where T : class
		{
			if (list.Contains(item))
			{
				return;
			}

			list.Add(item);
		}

		public static void AddUnique<T>(this List<T> list, in T item) where T : struct
		{
			if (list.Contains(item))
			{
				return;
			}

			list.Add(item);
		}
	}
}
