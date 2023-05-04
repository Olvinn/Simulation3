using Unity.Entities;

namespace Creatures.Plants
{
    [GenerateAuthoringComponent]
    public struct PlantComponent : IComponentData
    {
        public float age;
        public float maxAge;
        public float energy;
    }
}
