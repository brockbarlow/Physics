using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    [HideInInspector]public Vector3 velocity;
    [HideInInspector]public float mass;

    public void LateUpdate()
    {
        transform.position += velocity;
        transform.forward = velocity.normalized;
    }
}