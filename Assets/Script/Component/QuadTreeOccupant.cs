using Unity.Entities;

namespace Left4Dots.QuadTree
{
	public struct QuadTreeOccupant : IComponentData
	{
		public ushort m_partitionId;
	}
}
