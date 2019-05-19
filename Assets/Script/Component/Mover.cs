using Unity.Entities;
using Unity.Mathematics;

namespace Left4Dots.Component
{
	public struct Mover : IComponentData
	{
		public float3 m_velocity;
	}
}