using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    [HideInInspector]public Vector3 velocity;

    public void LateUpdate()
    {
        transform.position += velocity.normalized;
        transform.forward = velocity.normalized;
    }
}