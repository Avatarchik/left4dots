using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.Component
{
	public class MoverProxy : MonoBehaviour, IConvertGameObjectToEntity
	{
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var moverData = new Mover 
			{ 
				m_velocity = new float3(0.0f, 0.0f, 0.0f),
			};

			dstManager.AddComponentData(entity, moverData);
		}
	}
}