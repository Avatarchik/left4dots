using Unity.Entities;

namespace TranslatePrototype
{
	public struct TranslateSpawner : IComponentData
	{
		public Entity m_prefab;
		public int m_countX;
		public int m_countY;
	}
}