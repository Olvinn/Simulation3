using Scene;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Plants
{ 
    [AlwaysUpdateSystem]
    public partial class PlantSystem : SystemBase
    {
        private float _plantEnergyModifier = 1;
        private EndSimulationEntityCommandBufferSystem _end;

        protected override void OnCreate()
        {
            base.OnCreate();
            _end = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = UnityEngine.Time.deltaTime;
            float pem = _plantEnergyModifier;
            var manager = _end.CreateCommandBuffer();

            Entities.ForEach((ref PlantComponent plantComponent, ref NonUniformScale scale) =>
            {
                plantComponent.age += dt;
                if (plantComponent.age > plantComponent.maxAge)
                {
                    plantComponent.age = plantComponent.maxAge;
                    plantComponent.energy -= pem * dt;
                }
                else
                    plantComponent.energy += pem * dt;
                
                scale.Value =  Mathf.Pow(plantComponent.energy, 0.333f);
            }).ScheduleParallel();

            Entities.ForEach((Entity entity, ref PlantComponent plantComponent) =>
            {
                if (plantComponent.energy < 0)
                    manager.DestroyEntity(entity);
                
            }).Run();

            CreatePlant();
            
            _end.AddJobHandleForProducer(Dependency);
        }

        private void CreatePlant()
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            ComponentType[] comps =
            {
                typeof(PlantComponent),
                typeof(Translation),
                typeof(LocalToWorld),
                typeof(RenderBounds),
                typeof(RenderMesh),
                typeof(NonUniformScale)
            };
            var sphereArchetype = manager.CreateArchetype(comps);

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity entity = entityManager.CreateEntity(sphereArchetype);
            entityManager.SetComponentData(entity, new PlantComponent() { age = 0, energy = 0, maxAge = Random.Range(15, 60)});
            entityManager.SetComponentData(entity, new Translation() { Value = new float3(Random.Range(-100, 100),0,Random.Range(-100, 100))});
            entityManager.SetComponentData(entity, new LocalToWorld { Value = float4x4.identity });
            entityManager.AddComponentData(entity, new RenderBounds { Value = new AABB { Center = new float3(0f, 0f, 0f), Extents = new float3(1f, 1f, 1f) } });
            var description = new RenderMeshDescription(
                mesh : DataBank.singleton.visuals.plantMesh,
                material : DataBank.singleton.visuals.plantMaterial
            );
            RenderMeshUtility.AddComponents(entity, manager, description);
        }
    }
}
