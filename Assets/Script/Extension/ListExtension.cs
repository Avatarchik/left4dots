using System.Collections.Generic;

namespace Left4Dots.ListExtension
{
	public static class ListExtension
	{
		public static void AddUnique<T>(this List<T> list, T item)
		{
			if (list.Contains(item))
			{
				return;
			}

			list.Add(item);
		}
	}
}
