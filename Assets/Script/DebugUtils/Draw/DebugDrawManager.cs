using Left4Dots.ListExtension;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.DebugUtils.Draw
{
	public sealed class DebugDrawManager : MonoBehaviour
	{
		private static List<IDebugDrawable> s_pendingRegisteredDrawables = new List<IDebugDrawable>();
		private static List<IDebugDrawable> s_pendingUnregisteredDrawables = new List<IDebugDrawable>();

		// ----------------------------------------------------------------------------

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
			ProcessPendingRegisters();
			ProcessPendingUnregsiters();
			DebugDraw();
		}

		// ----------------------------------------------------------------------------

		private void ProcessPendingRegisters()
		{
			for (int i = 0, count = s_pendingRegisteredDrawables.Count; i < count; ++i)
			{
				if (null == s_pendingRegisteredDrawables[i])
				{
					continue;
				}

				long category = CategoryAsLong(s_pendingRegisteredDrawables[i].DebugDrawCategory);
				if (false == m_drawables.TryGetValue(category, out List<IDebugDrawable> drawables))
				{
					drawables = new List<IDebugDrawable>();
					m_drawables.Add(category, drawables);
				}

				drawables.AddUnique(s_pendingRegisteredDrawables[i]);
			}

			s_pendingRegisteredDrawables.Clear();
		}

		private void ProcessPendingUnregsiters()
		{
			for (int i = 0, count = s_pendingUnregisteredDrawables.Count; i < count; ++i)
			{
				if (null == s_pendingUnregisteredDrawables[i])
				{
					continue;
				}

				long category = CategoryAsLong(s_pendingUnregisteredDrawables[i].DebugDrawCategory);
				if (m_drawables.TryGetValue(category, out List<IDebugDrawable> drawables))
				{
					drawables.Remove(s_pendingUnregisteredDrawables[i]);
				}
			}

			s_pendingUnregisteredDrawables.Clear();
		}

		private void DebugDraw()
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

		public static void Register(IDebugDrawable drawable)
		{
			s_pendingRegisteredDrawables.AddUnique(drawable);
		}

		public static void Unregister(IDebugDrawable drawable)
		{
			s_pendingUnregisteredDrawables.AddUnique(drawable);
		}

		// ----------------------------------------------------------------------------

		public void DrawLine(EDebugDrawCategory category, float3 from, float3 to, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			Debug.DrawLine(from, to, colour, duration);
		}

		public void DrawAABox2D(EDebugDrawCategory category, float2 min, float2 max, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			float3 minV3 = new float3(min.x, 0.0f, min.y);
			float3 maxV3 = new float3(max.x, 0.0f, max.y);
			float3 minXmaxY = new float3(min.x, 0.0f, max.y);
			float3 maxXminY = new float3(max.x, 0.0f, min.y);
			
			DrawLine(category, minV3, minXmaxY, colour, duration);
			DrawLine(category, minV3, maxXminY, colour, duration);
			DrawLine(category, maxV3, minXmaxY, colour, duration);
			DrawLine(category, maxV3, maxXminY, colour, duration);
		}

		public void DrawAABox3D(EDebugDrawCategory category, float3 min, float3 max, Color colour, float duration = 0.0f)
		{
			if (false == IsCategoryEnabled(category))
			{
				return;
			}

			float3 minXminYmaxZ = new float3(min.x, min.y, max.z);
			float3 minXmaxYminZ = new float3(min.x, max.y, min.z);
			float3 maxXminYminZ = new float3(max.x, min.y, min.z);
			float3 minXmaxYmaxZ = new float3(min.x, max.y, max.z);
			float3 maxXmaxYminZ = new float3(max.x, max.y, min.z);
			float3 maxXminYmaxZ = new float3(max.x, min.y, max.z);

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