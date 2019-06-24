using Left4Dots.DebugUtils.Draw;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

// #SteveD >>> specify max depth, bounds, split trigger & merge trigger from config *somewhere*

namespace Left4Dots.QuadTree
{
	public class QuadTreeSystem : JobComponentSystem, IDebugDrawable
	{
		private NativeArray<QuadTreePartition> m_partitions;

		// ----------------------------------------------------------------------------

		protected override void OnCreateManager()
		{
			base.OnCreateManager();

			CreatePartitions(7); // #SteveD <<< replace magic number with config
		
			DebugDrawManager.Instance?.Register(this);
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
			
			// native memory
			int partitionArraySize = 0;
			for (int depth = 0; depth <= maxDepth; ++depth)
			{
				partitionArraySize += 1 << (depth * 2);
			}
			m_partitions = new NativeArray<QuadTreePartition>(partitionArraySize, Allocator.Persistent);

			CreatePartition(0, 0, 0);
			CreateChildPartitions(0, 0, maxDepth);
			ValidatePartitions();
		}

		private ushort GenerateUID(ushort parentUID, byte partition)
		{
			ushort uid = (ushort)((parentUID << 2) + 1);
			uid += partition;
			Debug.Assert(false == m_partitions[uid].IsCreated);

			return uid;
		}

		private void CreatePartitionRecursive(ushort parentUID, byte partition, byte depth, byte maxDepth)
		{
			ushort uid = GenerateUID(parentUID, partition);
			CreatePartition(uid, parentUID, depth);
			if (depth < maxDepth)
			{
				CreateChildPartitions(uid, depth, maxDepth);
			}
		}

		private void CreatePartition(ushort uid, ushort parentUID, byte depth)
		{
			m_partitions[uid] = new QuadTreePartition()
			{
				m_data = QuadTreePartition.k_isCreatedMask,
				m_partitionUID = uid,
				m_parentPartitionUID = parentUID,
				//m_minBounds <<--- #SteveD
				//m_maxBounds <<--- #SteveD
			};

			//Debug.LogFormat("[QuadTreeSystem::CreatePartitionRecursive] created partition {0} at depth {1} with parent {2}\n",
			//	uid, depth, parentUID);
		}

		private void CreateChildPartitions(ushort parentUID, byte parentDepth, byte maxDepth)
		{
			byte childDepth = (byte)(parentDepth + 1);
			for (byte i = 0; i < 4; ++i)
			{
				CreatePartitionRecursive(parentUID, i, childDepth, maxDepth);
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

		protected override void OnDestroyManager()
		{
			base.OnDestroyManager();

			if (m_partitions.IsCreated)
			{
				m_partitions.Dispose();
			}
		}

		// ----------------------------------------------------------------------------
		// IDebugDrawable -------------------------------------------------------------

		public EDebugDrawCategory DebugDrawCategory { get { return EDebugDrawCategory.QuadTree; } }

		public void DebugDraw()
		{
			DebugDrawManager.Instance?.DrawAABox2D(
				DebugDrawCategory, 
				new Vector2(-32.0f, -32.0f), 
				new Vector2(32.0f, 32.0f), 
				Color.red);

			DebugDrawManager.Instance?.DrawAABox3D(
				DebugDrawCategory, 
				new Vector3(-32.0f, -32.0f, -32.0f), 
				new Vector3(32.0f, 32.0f, 32.0f), 
				Color.green);
		}
	}
}