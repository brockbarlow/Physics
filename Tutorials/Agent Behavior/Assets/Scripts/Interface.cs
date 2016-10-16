using UnityEngine;

namespace Interface
{
    public interface Iboid
    {
        Vector3 velocity { get; set; }
        Vector3 position { get; set; }
        float mass { get; set; }
    }
}