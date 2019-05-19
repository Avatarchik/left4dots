using Unity.Entities;
using Unity.Mathematics;

namespace Left4Dots.Component
{
	public struct MoverSpawner : IComponentData
	{
		public Entity m_prefab;
		public int m_count;
		public Random m_random;
		public float3 m_maxMoverVelocity;
	}
}