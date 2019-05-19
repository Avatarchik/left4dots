using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Left4Dots.Component
{
	public class MoverSpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
	{
		[SerializeField] private GameObject m_prefab = null;
		[SerializeField] private int m_count = 100;
		[SerializeField] private uint m_randomSeed = 1337;
		[SerializeField] private float3 m_maxMoverVelocity = new float3(5.0f, 5.0f, 5.0f);

		// ----------------------------------------------------------------------------

		// Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
		public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
		{
			gameObjects.Add(m_prefab);
		}

		// ----------------------------------------------------------------------------

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var spawnerData = new MoverSpawner
			{
				// the referenced prefab will be converted due to DeclareReferencedPrefabs
				// here we simply map the game object to an entity reference to that prefab
				m_prefab = conversionSystem.GetPrimaryEntity(m_prefab),
				m_count = m_count,
				m_random = new Random(m_randomSeed),
				m_maxMoverVelocity = m_maxMoverVelocity,
			};
			dstManager.AddComponentData(entity, spawnerData);
		}
	}
}