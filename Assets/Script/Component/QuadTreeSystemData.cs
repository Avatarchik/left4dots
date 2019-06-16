using Unity.Entities;

namespace Left4Dots.Component
{
	public struct QuadTreeSystemData : IComponentData
	{
		public int m_maxDepth;
		public int m_splitTrigger;
		public int m_mergeTrigger;
	}
}