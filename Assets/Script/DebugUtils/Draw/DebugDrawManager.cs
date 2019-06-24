using Left4Dots.Extension;
using System.Collections.Generic;
using UnityEngine;

// #SteveD >>> ComponentSystem::OnCreateManager is being called before DebugDrawManager::Awake

namespace Left4Dots.DebugUtils.Draw
{
	public sealed class DebugDrawManager : MonoBehaviour
	{
		private static DebugDrawManager m_instance = null;
		public static DebugDrawManager Instance
		{
			get 
			{ 
				return m_instance;
			}
		}

		// ----------------------------------------------------------------------------

		[SerializeField] private List<EDebugDrawCategory> m_enabledByDefault = new List<EDebugDrawCategory>();

		// ----------------------------------------------------------------------------

		private Dictionary<long, List<IDebugDrawable>> m_drawables = new Dictionary<long, List<IDebugDrawable>>();
		private long m_activeCategories = 0;

		// ----------------------------------------------------------------------------

		private void Awake()
		{
			if (null != m_instance)
			{
				DestroyImmediate(this);
				return;
			}

			m_instance = this;
		}

		private void Start()
		{
			int enabledCount = m_enabledByDefault.Count;
			for (int i = 0; i < enabledCount; ++i)
			{
				EnableCategory(m_enabledByDefault[i]);
			}
		}

		private void LateUpdate()
		{
			foreach (var kvp in m_drawables)
			{
				if (false == IsCategoryEnabled(kvp.Key))
				{
					continue;
				}

				var drawables = kvp.Value;
				int drawableCount = drawables.Count;

				for (int d = 0; d < drawableCount; ++d)
				{
					drawables[d].DebugDraw();
				}
			}
		}

		// ----------------------------------------------------------------------------

		public static long CategoryAsLong(EDebugDrawCategory category)
		{
			return (long)category;
		}
		
		private bool IsCategoryEnabled(EDebugDrawCategory category)
		{
			return IsCategoryEnabled(CategoryAsLong(category));
		}

		private bool IsCategoryEnabled(long category)
		{
			return (m_activeCategories & category) > 0;
		}

		public void EnableCategory(EDebugDrawCategory category)
		{
			m_activeCategories |= CategoryAsLong(category);
		}

		public void DisableCategory(EDebugDrawCategory category)
		{
			m_activeCategories &= ~CategoryAsLong(category);
		}

		// ----------------------------------------------------------------------------

		public void Register(IDebugDrawable drawable)
		{
			EDebugDrawCategory category = drawable.DebugDrawCategory;
			long longCategory = CategoryAsLong(category);

			if (false == m_drawables.TryGetValue(longCategory, out List<IDebugDrawable> drawables))
			{
				drawables = new List<IDebugDrawable>();
				m_drawables.Add(longCategory, drawables);
			}

			drawables.AddUnique(drawable);
		}

		public void Unregister(IDebugDrawable drawable)
		{
			EDebugDrawCategory category = drawable.DebugDrawCategory;
			long longCategory = CategoryAsLong(category);

			if (m_drawables.TryGetValue(longCategory, out List<IDebugDrawable> drawables))
			{
				drawables.Remove(drawable);
			}
		}

		// ----------------------------------------------------------------------------

		public void DrawLine(EDebugDrawCategory category, Vector3 from, Vector3 to, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			Debug.DrawLine(from, to, colour, duration);
		}

		public void DrawAABox2D(EDebugDrawCategory category, Vector2 min, Vector2 max, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			Vector2 minXmaxY = new Vector2(max.x, min.y);
			Vector2 maxXminY = new Vector2(min.x, max.y);
			
			DrawLine(category, min, minXmaxY, colour, duration);
			DrawLine(category, min, maxXminY, colour, duration);
			DrawLine(category, max, minXmaxY, colour, duration);
			DrawLine(category, max, maxXminY, colour, duration);
		}

		public void DrawAABox3D(EDebugDrawCategory category, Vector3 min, Vector3 max, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			Vector3 minXminYmaxZ = new Vector3(min.x, min.y, max.z);
			Vector3 minXmaxYminZ = new Vector3(min.x, max.y, min.z);
			Vector3 maxXminYminZ = new Vector3(max.x, min.y, min.z);
			Vector3 minXmaxYmaxZ = new Vector3(min.x, max.y, max.z);
			Vector3 maxXmaxYminZ = new Vector3(max.x, max.y, min.z);
			Vector3 maxXminYmaxZ = new Vector3(max.x, min.y, max.z);

			DrawLine(category, min, minXminYmaxZ, colour, duration);
			DrawLine(category, min, minXmaxYminZ, colour, duration);
			DrawLine(category, min, maxXminYminZ, colour, duration);
			DrawLine(category, max, minXmaxYmaxZ, colour, duration);
			DrawLine(category, max, maxXmaxYminZ, colour, duration);
			DrawLine(category, max, maxXminYmaxZ, colour, duration);
			DrawLine(category, minXmaxYminZ, minXmaxYmaxZ, colour, duration);
			DrawLine(category, minXmaxYminZ, maxXmaxYminZ, colour, duration);
			DrawLine(category, maxXminYmaxZ, minXminYmaxZ, colour, duration);
			DrawLine(category, maxXminYmaxZ, maxXminYminZ, colour, duration);
			DrawLine(category, maxXminYminZ, maxXmaxYminZ, colour, duration);
			DrawLine(category, minXminYmaxZ, minXmaxYmaxZ, colour, duration);
		}
	}
}