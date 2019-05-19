using Left4Dots.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Left4Dots.ComponentSystem
{
	public class MoverResetSystem : JobComponentSystem
	{
		[BurstCompile]
		struct MoverResetJob : IJobForEach<Translation, Mover>
		{
			public float m_maxDistanceSq;

			// --------------------------------------------------------------------

			public void Execute(ref Translation translation, [ReadOnly] ref Mover mover)
			{
				float3 position = translation.Value;

				// #SteveD >>> get magic boundary number from prefab/gameobject..
				if (math.lengthsq(position) > m_maxDistanceSq)
				{
					translation.Value = new float3(0.0f, 0.0f, 0.0f);
				}
			}
		}

		// ----------------------------------------------------------------------------
		// ----------------------------------------------------------------------------

		protected override JobHandle OnUpdate(JobHandle inputDependencies)
		{
			var moveJob = new MoverResetJob 
			{
				m_maxDistanceSq = 5.0f * 5.0f,
			};

			return moveJob.Schedule(this, inputDependencies);
		}
	}
}