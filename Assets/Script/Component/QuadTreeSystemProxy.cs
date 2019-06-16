using Unity.Entities;
using UnityEngine;

namespace Left4Dots.Component
{
	public class QuadTreeSystemProxy : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private int m_maxDepth = 7;
		[SerializeField] private int m_splitTrigger = 1024;
		[SerializeField] private int m_mergeTrigger = 512;

		// ----------------------------------------------------------------------------

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			QuadTreeSystemData quadTreeData = new QuadTreeSystemData
			{
				m_maxDepth = m_maxDepth,
				m_splitTrigger = m_splitTrigger,
				m_mergeTrigger = m_mergeTrigger,
			};

			dstManager.AddComponentData(entity, quadTreeData);
		}
	}
}