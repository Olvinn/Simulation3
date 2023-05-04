using Data;
using UnityEngine;

namespace Scene
{
    public class DataBank : MonoBehaviour
    {
        public static DataBank singleton;

        public Visuals visuals;

        private void Awake()
        {
            singleton = this;
        }
    }
}
