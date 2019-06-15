using Unity.Entities;

namespace Left4Dots.Component
{
	public struct QuadTreeOccupant : ISharedComponentData
	{
		public ushort m_partitionId;
	}
}
