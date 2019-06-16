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

			CreatePartitionRecursive(0, 0, maxDepth, 1, 0);
			ValidatePartitions();
		}

		// #steveD >>> fix, refactor
		private void CreatePartitionRecursive(int splitNumber, int depth, int maxDepth, int parentSplitNumber, ushort parentUID)
		{
			// generate uid
			ushort uid = (ushort)(splitNumber + ((parentSplitNumber - 1) * 4));
			for (int i = 1; i < depth; ++i)
			{
				uid += (ushort)(1 << (i * 2));
			}

			// create partition
			m_partitions[uid] = new QuadTreePartition()
			{
				m_data = QuadTreePartition.k_isCreatedMask,
				m_partitionUID = uid,
				m_parentPartitionUID = parentUID,
			};

			// create children if we're not at max depth
			if (depth < maxDepth)
			{
				int childDepth = depth + 1;
				for (int i = 1; i <= 4; ++i)
				{
					CreatePartitionRecursive(i, childDepth, maxDepth, splitNumber, uid);
				}
			}
		}
		// <<<<<<<<<<<

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