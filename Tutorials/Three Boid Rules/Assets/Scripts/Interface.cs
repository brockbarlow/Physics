using UnityEngine;

namespace Interface
{
    public interface IBoid
    {
        Vector3 velocity { get; set; }
        Vector3 position { get; set; }
        float mass { get; set; }
    }
}