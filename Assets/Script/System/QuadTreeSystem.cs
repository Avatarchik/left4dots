using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

// #SteveD >>> create quad tree system from gameobject so we can specify max depth, split trigger, merge trigger
//			>> see EntitySpawnerSystem for pulling in proxy variables

namespace Left4Dots.System
{
	public class QuadTreeSystem : JobComponentSystem
	{
		private NativeArray<QuadTreePartition> m_partitions;

		// ----------------------------------------------------------------------------

		protected override void OnCreateManager()
		{
			base.OnCreateManager();

			CreatePartitions(2);
		}

		private void CreatePartitions(byte maxDepth)
		{
			// Eg. Max depth: 7
			//	 0			(0)
			// + 1 << 2		(4)
			// + 1 << 4		(16)
			// + 1 << 6		(64)
			// + 1 << 8		(256)
			// + 1 << 10	(1,024)
			// + 1 << 12	(4,096)
			// + 1 << 14	(16,384)
			// ----------------------
			// =			21,844
			// ----------------------

			int partitionArraySize = 0;
			for (int depth = 0; depth <= maxDepth; ++depth)
			{
				partitionArraySize += 1 << (depth * 2);
			}
			m_partitions = new NativeArray<QuadTreePartition>(partitionArraySize, Allocator.Persistent);

			CreatePartitionRecursive(0, 0, maxDepth, 0, 0);
			ValidatePartitions();
		}

		// #SteveD >>> refactor method signature
		// #SteveD >>> fix (see initialisation logging). Add test?
		private void CreatePartitionRecursive(int quadrant, int depth, int maxDepth, int parentQuadrant, ushort parentUID)
		{
			// generate uid
			ushort uid = 0;
			for (int i = 1; i < depth - 1; ++i)
			{
				uid += (ushort)(1 << (i * 2));
			}
			uid += (ushort)(parentQuadrant * 4);
			uid += (ushort)quadrant;

			// create partition
			m_partitions[uid] = new QuadTreePartition()
			{
				m_data = QuadTreePartition.k_isCreatedMask,
				m_partitionUID = uid,
				m_parentPartitionUID = parentUID,
			};

			// create children, if we're not at max depth
			if (depth < maxDepth)
			{
				for (int i = 0; i <= 3; ++i)
				{
					CreatePartitionRecursive(i, depth + 1, maxDepth, quadrant, uid);
				}
			}
		}
		
		private void ValidatePartitions()
		{
			int countCreated = 0;
			int countInvalid = 0;
			
			for (int i = 0; i < m_partitions.Length; ++i)
			{
				if (m_partitions[i].IsCreated)
				{
					++countCreated;
				}
				else
				{
					Debug.LogWarningFormat("[QuadTreeSystem::ValidatePartitions] invalid partition at index: {0}", i);
					++countInvalid;
				}
			}

			Debug.LogFormat("[QuadTreeSystem::ValidatePartitions] count created: {0}", countCreated);
			Debug.LogFormat("[QuadTreeSystem::ValidatePartitions] count invalid: {0}", countInvalid);
			
			Debug.Assert(countCreated + countInvalid == m_partitions.Length);
			Debug.Assert(countInvalid == 0);
		}

		// ----------------------------------------------------------------------------

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			return inputDeps;
		}

		// ----------------------------------------------------------------------------

		protected override void OnDestroyManager()
		{
			base.OnDestroyManager();

			if (m_partitions.IsCreated)
			{
				m_partitions.Dispose();
			}
		}
	}
}