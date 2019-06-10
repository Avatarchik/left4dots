using Unity.Entities;
using Unity.Jobs;

namespace Left4Dots.System
{
	public class QuadTreeSystem : JobComponentSystem
	{
		// create
		// split
		// merge
		// assign

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			return inputDeps;
		}
	}
}