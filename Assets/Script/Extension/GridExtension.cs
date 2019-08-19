using Left4Dots.Container;
using Left4Dots.DebugUtils.Draw;
using Left4Dots.MathExtension;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.GridExtension
{
	public static class GridExtension
	{
		public static void DebugDraw(this UniformGrid grid, EDebugDrawCategory category, Color colour, float duration)
		{
			int minX = grid.m_min.x;
			int maxX = grid.m_max.x;
			int minY = grid.m_min.y;
			int maxY = grid.m_max.y;
			int granularity = grid.m_granularity;

			for (int x = minX; x <= maxX; x += granularity)
			{
				for (int y = minY; y <= maxY; y += granularity)
				{
					float3 centre = new float3(x, 0.0f, y);

					// N
					if (y < maxY)
					{
						DebugDrawManager.Instance.DrawLine(category, centre, new float3(x, 0.0f, y + granularity), colour, duration);
					}

					// S
					if (y > minY)
					{
						DebugDrawManager.Instance.DrawLine(category, centre, new float3(x, 0.0f, y - granularity), colour, duration);
					}

					// E
					if (x < maxX)
					{
						DebugDrawManager.Instance.DrawLine(category, centre, new float3(x + granularity, 0.0f, y), colour, duration);
					}

					// W
					if (x > minX)
					{
						DebugDrawManager.Instance.DrawLine(category, centre, new float3(x - granularity, 0.0f, y), colour, duration);
					}
				}
			}
		}

		public static bool TryGetCellCoordinate(this UniformGrid grid, float2 position, out int2 cellCoordinate)
		{
			cellCoordinate = new int2(
				((int)position.x - grid.m_min.x) / grid.m_granularity,
				((int)position.y - grid.m_min.y) / grid.m_granularity
			);

			return position.x >= grid.m_min.x
				&& position.x <= grid.m_max.x
				&& position.y >= grid.m_min.y
				&& position.y <= grid.m_max.y;
		}

		public static string ToStringCompact(this UniformGrid grid)
		{
			return string.Format("{0} to {1} at {2} granularity\n",
				grid.m_min.ToStringCompact(),
				grid.m_max.ToStringCompact(),
				grid.m_granularity);
		}
	}
}