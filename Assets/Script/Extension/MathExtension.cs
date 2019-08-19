using Unity.Mathematics;

namespace Left4Dots.MathExtension
{
	public static class MathExtension
	{
		// ----------------------------------------------------------------------------
		// int2

		public static float2 ToFloat2(this int2 iv2)
		{
			return new float2(iv2.x, iv2.y);
		}

		public static int3 ToInt3XY(this int2 iv2)
		{
			return new int3(iv2.x, iv2.y, 0);
		}

		public static int3 ToInt3XZ(this int2 iv2)
		{
			return new int3(iv2.x, 0, iv2.y);
		}

		public static float3 ToFloat3XY(this int2 iv2)
		{
			return new float3(iv2.x, iv2.y, 0.0f);
		}

		public static float3 ToFloat3XZ(this int2 iv2)
		{
			return new float3(iv2.x, 0.0f, iv2.y);
		}

		public static string ToStringCompact(this int2 iv2)
		{
			return string.Format("({0}, {1})", iv2.x, iv2.y);
		}

		// ----------------------------------------------------------------------------
		// float2

		public static int2 ToInt2(this float2 fv2)
		{
			return new int2((int)fv2.x, (int)fv2.y);
		}

		public static float3 ToFloat3XY(this float2 fv2)
		{
			return new float3(fv2.x, fv2.y, 0.0f);
		}

		public static float3 ToFloat3XZ(this float2 fv2)
		{
			return new float3(fv2.x, 0.0f, fv2.y);
		}

		public static string ToStringCompact(this float2 fv2)
		{
			return string.Format("({0:0.00}, {1:0.00})", fv2.x, fv2.y);
		}
	}
}
