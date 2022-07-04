using UnityEngine;

namespace TdTest.Targets
{
    public interface IHittable
    {
        public Transform Transform { get; }
    }
}