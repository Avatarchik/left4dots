using Left4Dots.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Left4Dots.ComponentSystem
{
	public class MoverMoveSystem : JobComponentSystem
	{
		[BurstCompile]
		struct MoverMoveJob : IJobForEach<Translation, Mover>
		{
			public float m_deltaTime;

			// ------------------------------------------------------------------------

			public void Execute(ref Translation translation, [ReadOnly] ref Mover mover)
			{
				float3 position = translation.Value;
				position += mover.m_velocity * m_deltaTime;
				translation.Value = position;
			}
		}

		// ----------------------------------------------------------------------------
		// ----------------------------------------------------------------------------

		protected override JobHandle OnUpdate(JobHandle inputDependencies)
		{
			var moveJob = new MoverMoveJob
			{
				m_deltaTime = Time.deltaTime
			};

			return moveJob.Schedule(this, inputDependencies);
		}
	}
}