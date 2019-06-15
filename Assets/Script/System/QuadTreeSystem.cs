using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Left4Dots.System
{
	public class QuadTreeSystem : JobComponentSystem
	{
		private const uint k_occupantCountMask			= 0b_00000000_11111111_11111111_11111111;
		private const byte k_occupantCountShift			= 0;

		private const uint k_depthMask					= 0b_00000111_00000000_00000000_00000000;
		private const byte k_depthShift					= 24;

		private const uint k_isSplitMask				= 0b_00001000_00000000_00000000_00000000;
		private const byte k_isSplitShift				= 27;

		// ----------------------------------------------------------------------------

		public struct QuadTreePartition
		{
			public half2 m_minBounds;
			public half2 m_maxBounds;
			public ushort m_partitionId;
			public ushort m_parentPartitionId;
			public uint m_data;

			// ------------------------------------------------------------------------

			private uint GetData(uint mask, byte shift)
			{
				return (m_data & mask) >> shift;
			}
			
			private void SetData(uint value, uint mask, byte shift)
			{
				m_data &= ~(~0u & mask);
				m_data |= (value << shift);
			}
			
			// ------------------------------------------------------------------------

			public uint OccupantCount
			{
				get { return GetData(k_occupantCountMask, k_occupantCountShift); }
				set { SetData(value, k_occupantCountMask, k_occupantCountShift); }
			}

			public uint Depth
			{
				get { return GetData(k_depthMask, k_depthShift); }
				set { SetData(value, k_depthMask, k_depthShift); }
			}

			public bool IsSplit
			{
				get { return GetData(k_isSplitMask, k_isSplitShift) > 0; }
				set { SetData((uint)(value ? 1 : 0), k_isSplitMask, k_isSplitShift); }
			}
		}
		
		// ----------------------------------------------------------------------------

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			return inputDeps;
		}

		// ----------------------------------------------------------------------------

#if UNITY_EDITOR

		public void RunPartitionTest()
		{
			QuadTreePartition p = new QuadTreePartition()
			{
				m_minBounds = new half2(),
				m_maxBounds = new half2(),
				m_partitionId = 0,
				m_parentPartitionId = 0,
				m_data = 0,
			};

			Debug.Log("QuadTreePartition Test\n");

			Debug.Log("Depth\n");
			Debug.LogFormat("   Depth: {0}\n", p.Depth);
			p.Depth = 3;
			Debug.LogFormat("   Depth: {0}\n", p.Depth);
			Debug.Assert(p.Depth == 3);
			p.Depth = 0;
			Debug.LogFormat("   Depth: {0}\n", p.Depth);
			Debug.Assert(p.Depth == 0);
			p.Depth = 7;
			Debug.LogFormat("   Depth: {0}\n", p.Depth);
			Debug.Assert(p.Depth == 7);
			Debug.Log("--------------------------------------------------\n");

			Debug.Log("OccupantCount\n");
			Debug.LogFormat("   OccupantCount: {0}\n", p.OccupantCount);
			p.OccupantCount = 1_234_567;
			Debug.LogFormat("   OccupantCount: {0}\n", p.OccupantCount);
			Debug.Assert(p.OccupantCount == 1_234_567);
			p.OccupantCount = 89;
			Debug.LogFormat("   OccupantCount: {0}\n", p.OccupantCount);
			Debug.Assert(p.OccupantCount == 89);
			p.OccupantCount = 0;
			Debug.LogFormat("   OccupantCount: {0}\n", p.OccupantCount);
			Debug.Assert(p.OccupantCount == 0);
			p.OccupantCount = 1;
			Debug.LogFormat("   OccupantCount: {0}\n", p.OccupantCount);
			Debug.Assert(p.OccupantCount == 1);
			Debug.Log("--------------------------------------------------\n");

			Debug.Log("Split\n");
			Debug.LogFormat("   Partition IsSplit: {0}\n", p.IsSplit);
			p.IsSplit = true;
			Debug.LogFormat("   Partition IsSplit: {0}\n", p.IsSplit);
			Debug.Assert(p.IsSplit);
			p.IsSplit = false;
			Debug.LogFormat("   Partition IsSplit: {0}\n", p.IsSplit);
			Debug.Assert(false == p.IsSplit);
			Debug.Log("--------------------------------------------------\n");
		}

#endif // UNITY_EDITOR
	}
}