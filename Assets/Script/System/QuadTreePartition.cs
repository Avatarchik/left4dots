using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

using half2 = Unity.Mathematics.half2;
using Random = Unity.Mathematics.Random;

namespace Left4Dots.QuadTree
{
	public struct QuadTreePartition
	{
		private const uint k_occupantCountMask = 0b_00000000_11111111_11111111_11111111;
		private const byte k_occupantCountShift = 0;

		private const uint k_depthMask = 0b_00000111_00000000_00000000_00000000;
		private const byte k_depthShift = 24;

		private const uint k_isSplitMask = 0b_00001000_00000000_00000000_00000000;
		private const byte k_isSplitShift = 27;

		public const uint k_isCreatedMask = 0b_10000000_00000000_00000000_00000000;

		// ----------------------------------------------------------------------------

		public half2 m_minBounds;
		public half2 m_maxBounds;
		public ushort m_partitionUID;
		public ushort m_parentPartitionUID;
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

		public bool IsCreated => (m_data & k_isCreatedMask) > 0;

		// ----------------------------------------------------------------------------

#if UNITY_EDITOR

		[MenuItem("Test Suite/QuadTree/Partition")]
		private static void RunPartitionTest()
		{
			QuadTreePartition p = new QuadTreePartition()
			{
				m_minBounds = new half2(),
				m_maxBounds = new half2(),
				m_partitionUID = 0,
				m_parentPartitionUID = 0,
				m_data = 0,
			};

			Random rand = new Random();
			rand.InitState();

			Debug.Log(" -------------------------------- QuadTreePartition Test --------------------------------\n");

			Debug.Log("Depth\n");
			Debug.LogFormat("\tInitial Depth: {0}\n", p.Depth);

			uint testDepth = 0;
			for (int i = 1; i <= 3; ++i)
			{
				testDepth = (uint)rand.NextInt(0, 7);
				Debug.LogFormat("\tSetting Depth to: {0}\n", testDepth);
				p.Depth = testDepth;
				Debug.LogFormat("\tDepth: {0}\n", p.Depth);
				Debug.Assert(p.Depth == testDepth);
			}

			Debug.Log(" ----------------------------------------------------------------------------------------\n");

			Debug.Log("OccupantCount\n");
			Debug.LogFormat("\tinitial OccupantCount: {0}\n", p.OccupantCount);

			uint testOccupantCount = 0;
			for (int i = 1; i <= 3; ++i)
			{
				testOccupantCount = (uint)rand.NextInt(0, 10_000_000);
				Debug.LogFormat("\tSetting OccupantCount to: {0}\n", testOccupantCount);
				p.OccupantCount = testOccupantCount;
				Debug.LogFormat("\tOccupantCount: {0}\n", p.OccupantCount);
				Debug.Assert(p.OccupantCount == testOccupantCount);
			}

			Debug.Log(" ----------------------------------------------------------------------------------------\n");

			Debug.Log("Split\n");
			Debug.LogFormat("\tInitial IsSplit: {0}\n", p.IsSplit);

			bool testIsSplit = false;
			for (int i = 1; i <= 3; ++i)
			{
				testIsSplit = rand.NextBool();
				Debug.LogFormat("\tSetting IsSplit to: {0}\n", testIsSplit);
				p.IsSplit = testIsSplit;
				Debug.LogFormat("\tIsSplit: {0}\n", p.IsSplit);
				Debug.Assert(p.IsSplit == testIsSplit);
			}

			Debug.Log(" ----------------------------------------------------------------------------------------\n");
		}

#endif // UNITY_EDITOR
	}
}