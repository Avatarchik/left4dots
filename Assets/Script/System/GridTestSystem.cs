using Left4Dots.Container;
using Left4Dots.DebugUtils.Draw;
using Left4Dots.GridExtension;
using Left4Dots.MathExtension;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.TestSystem
{
	public class GridTestSystem : JobComponentSystem, IDebugDrawable
	{
		private UniformGrid m_grid;

		// ----------------------------------------------------------------------------

		protected override void OnCreate()
		{
			base.OnCreate();

			m_grid = new UniformGrid(
				new int2(-100, -100),
				new int2(100, 100),
				7);

			Debug.LogFormat("[GridTestSystem::OnCreate] Created Grid: {0}\n", m_grid.ToStringCompact());
			//TestGrid();

			DebugDrawManager.Register(this);
		}
		
		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			return inputDeps;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// ----------------------------------------------------------------------------
		// IDebugDrawable

		public EDebugDrawCategory DebugDrawCategory { get { return EDebugDrawCategory.Test; } }

		public void DebugDraw()
		{
			m_grid.DebugDraw(EDebugDrawCategory.Test, Color.green, 0.0f);
		}

		// ----------------------------------------------------------------------------

		private void TestGrid()
		{
			var rand = new Unity.Mathematics.Random(1337);
			for (int x = -103; x <= 103; x += m_grid.m_granularity)
			{
				for (int y = -103; y <= 103; y += m_grid.m_granularity)
				{
					float fx = rand.NextFloat(0.0f, 0.999f);
					float fy = rand.NextFloat(0.0f, 0.999f);
					float2 position = new float2(x + fx, y + fy);
					if (m_grid.TryGetCellCoordinate(position, out int2 cellCoordinate))
					{
						Debug.LogFormat("[TestSystem::TestGrid] Grid contains {0} at cell: {1}\n", position.ToStringCompact(), cellCoordinate.ToStringCompact());
					}
					else
					{
						Debug.LogFormat("[TestSystem::TestGrid] Grid does not contain {0}\n", position.ToStringCompact());
					}
				}
			}
		}
	}
}