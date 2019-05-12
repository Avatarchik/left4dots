using Unity.Entities;
using UnityEngine;

namespace TranslatePrototype
{
	public class TranslateProxy : MonoBehaviour, IConvertGameObjectToEntity
	{
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var translateData = new Translate
			{
			};
			dstManager.AddComponentData(entity, translateData);
		}
	}
}