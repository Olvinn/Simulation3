using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Simulation/Visuals")]
    public class Visuals : ScriptableObject
    {
        public Mesh plantMesh;
        public Material plantMaterial;
    }
}
