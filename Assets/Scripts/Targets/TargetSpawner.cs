using UnityEngine;

namespace TdTest.Targets
{
    public class TargetSpawner: MonoBehaviour
    {
        [SerializeField] private Target prefab;
        public Target Spawn()
        {
            // Debug.LogError("Not implemented yet");
            var target = Instantiate(prefab);
            return target;
        }
    }
}