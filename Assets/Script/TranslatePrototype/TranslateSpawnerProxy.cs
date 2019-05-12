using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TranslatePrototype
{
	public class TranslateSpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
	{
		public GameObject m_prefab;
		public int m_countX;
		public int m_countY;

		// ----------------------------------------------------------------------------

		// Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
		public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
		{
			gameObjects.Add(m_prefab);
		}

		// ----------------------------------------------------------------------------

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var spawnerData = new TranslateSpawner
			{
				// the referenced prefab will be converted due to DeclareReferencedPrefabs
				// here we simply map the game object to an entity reference to that prefab
				m_prefab = conversionSystem.GetPrimaryEntity(m_prefab),
				m_countX = m_countX,
				m_countY = m_countY
			};
			dstManager.AddComponentData(entity, spawnerData);
		}
	}
}