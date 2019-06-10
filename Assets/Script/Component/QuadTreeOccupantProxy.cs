using Unity.Entities;
using UnityEngine;

namespace Left4Dots.Component
{
	public class QuadTreeOccupantProxy : MonoBehaviour, IConvertGameObjectToEntity
	{
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			QuadTreeOccupant occupantData = new QuadTreeOccupant
			{
				m_partitionId = 0
			};

			dstManager.AddComponentData(entity, occupantData);
		}
	}
}