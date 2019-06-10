using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.Component
{
	public class EntitySpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
	{
		[SerializeField] private GameObject m_prefab = null;
		[SerializeField] private int2 m_minBounds = new int2(-100, -100);
		[SerializeField] private int2 m_maxBounds = new int2(100, 100);

		// ----------------------------------------------------------------------------

		// referenced prefabs have to be declared so that the conversion system knows about them ahead of time
		public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
		{
			gameObjects.Add(m_prefab);
		}

		// ----------------------------------------------------------------------------

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var spawnerData = new EntitySpawner
			{
				// the referenced prefab will be converted due to DeclareReferencedPrefabs
				// here we simply map the game object to an entity reference to that prefab
				m_prefab = conversionSystem.GetPrimaryEntity(m_prefab),

				m_minBounds = m_minBounds,
				m_maxBounds = m_maxBounds,
			};

			dstManager.AddComponentData(entity, spawnerData);
		}
	}
}