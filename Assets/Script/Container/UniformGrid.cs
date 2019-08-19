using Unity.Mathematics;

namespace Left4Dots.Container
{
	public readonly struct UniformGrid
	{
		public readonly int2 m_min;
		public readonly int2 m_max;
		public readonly int m_granularity;

		// --------------------------------------------------------------------------------

		public UniformGrid(int2 min, int2 max, int granularity)
		{
			m_min = min;
			m_granularity = granularity;

			m_max = new int2(
				math.max(min.x, max.x), 
				math.max(min.y, max.y));

			// expand to handle granularity
			int2 size = max - min;
			int expandX = granularity - (size.x % granularity);
			int expandY = granularity - (size.y % granularity);

			if (expandX > 0 
				&& expandX < granularity)
			{
				int halfExpandX = expandX / 2;
				m_max.x += halfExpandX + (expandX % 2);
				m_min.x -= halfExpandX;
			}
			if (expandY > 0 
				&& expandY < granularity)
			{
				int halfExpandY = expandY / 2;
				m_max.y += halfExpandY + (expandX % 2);
				m_min.y -= halfExpandY;
			}
		}
	}
}