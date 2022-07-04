using UnityEngine;

namespace TdTest.Upgrades
{
    public class Weapon : MonoBehaviour
    {
        public Vector3 startPosition;
        public int startLayer;
        
        public bool TryUpgrade(Weapon weaponToUpgrade)
        {
            Debug.LogError("Not implemented yet");
            return false;
        }
    }
}