using Unity.Entities;
using Unity.Mathematics;

namespace Left4Dots.Component
{
	public struct EntitySpawner : IComponentData
	{
		public Entity m_prefab;
		public int2 m_minBounds;
		public int2 m_maxBounds;
	}
}