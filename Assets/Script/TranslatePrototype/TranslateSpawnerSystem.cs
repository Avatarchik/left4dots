using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TranslatePrototype
{
	// JobComponentSystems can run on worker threads
	// however, creating and removing Entities can only be done on the main thread to prevent race conditions
	// TranslateSpawnerSystem system uses an EntityCommandBuffer to defer tasks that can't be done inside the Job
	public class TranslateSpawnerSystem : JobComponentSystem
	{
		struct SpawnJob : IJobForEachWithEntity<TranslateSpawner, LocalToWorld>
		{
			public EntityCommandBuffer.Concurrent m_commandBuffer;

			// ------------------------------------------------------------------------

			public void Execute(
				Entity entity, 
				int index, 
				[ReadOnly] ref TranslateSpawner spawner,
				[ReadOnly] ref LocalToWorld location)
			{
				for (int x = 0; x < spawner.m_countX; x++)
				{
					for (int y = 0; y < spawner.m_countY; y++)
					{
						// instantiate
						var instance = m_commandBuffer.Instantiate(index, spawner.m_prefab);
						
						// position
						var position = math.transform(
							location.Value,
							new float3(0.0f, 0.0f, 0.0f)
						);

						// set translation component
						m_commandBuffer.SetComponent(
							index, 
							instance, 
							new Translation 
							{ 
								Value = position 
							}
						);
					}
				}

				// destroy spawner
				m_commandBuffer.DestroyEntity(index, entity);
			}
		}

		// ----------------------------------------------------------------------------
		// ----------------------------------------------------------------------------

		// BeginInitializationEntityCommandBufferSystem is used to create a command buffer which will then be played back
		// when that barrier system executes
		// though the instantiation command is recorded in the SpawnJob, it's not actually processed ('played back')
		// until the corresponding EntityCommandBufferSystem is updated. To ensure that the transform system has a chance
		// to run on the newly-spawned entities before they're rendered for the first time, the TranslateSpawnerSystem
		// will use the BeginSimulationEntityCommandBufferSystem to play back its commands. This introduces a one-frame lag
		// between recording the commands and instantiating the entities, but in practice this is usually not noticeable.
		private BeginInitializationEntityCommandBufferSystem m_entityCommandBufferSystem = null;

		// ----------------------------------------------------------------------------

		protected override void OnCreate()
		{
			// cache BeginInitializationEntityCommandBufferSystem
			m_entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
		}

		// ----------------------------------------------------------------------------

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			// instead of performing structural changes directly, a Job can add a command to an EntityCommandBuffer to 
			// perform such changes on the main thread after the Job has finished
			// command buffers allow you to perform any, potentially costly, calculations on a worker thread, while 
			// queuing up the actual insertions and deletions for later

			// schedule the job that will add Instantiate commands to the EntityCommandBuffer
			var spawnJob = new SpawnJob
			{
				m_commandBuffer = m_entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
			};

			var spawnJobHandle = spawnJob.Schedule(this, inputDeps);


			// SpawnJob runs in parallel with no sync point until the barrier system executes
			// when the barrier system executes we want to complete the SpawnJob and then play back the commands (creating 
			// the entities and placing them)
			// we need to tell the barrier system which job it needs to complete before it can play back the commands
			m_entityCommandBufferSystem.AddJobHandleForProducer(spawnJobHandle);
			return spawnJobHandle;
		}
	}
}