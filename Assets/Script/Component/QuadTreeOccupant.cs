using Unity.Entities;

namespace Left4Dots.Component
{
	public struct QuadTreeOccupant : IComponentData
	{
		public uint m_partitionId;
	}
}
