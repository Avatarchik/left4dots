using Unity.Entities;

namespace Left4Dots.Component
{
	public struct QuadTreeOccupant : IComponentData
	{
		public ushort m_partitionId;
	}
}
