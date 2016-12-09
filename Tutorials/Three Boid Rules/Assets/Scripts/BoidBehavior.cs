namespace Assets.Scripts
{
    using UnityEngine;

    public class BoidBehavior : MonoBehaviour
    {
        [HideInInspector]public Vector3 Velocity;
        public float Mass;

        public void LateUpdate()
        {
            transform.position += Velocity;
            transform.forward = Velocity.normalized;
        }
    }
}