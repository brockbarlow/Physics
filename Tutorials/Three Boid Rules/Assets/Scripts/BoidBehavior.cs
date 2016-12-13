namespace Assets.Scripts
{   //required usings
    using UnityEngine;

    public class BoidBehavior : MonoBehaviour
    {
        public Vector3 Velocity;
        public float Mass;

        public void LateUpdate()
        {
            transform.position += Velocity; //add velocity value to the transform position
            transform.forward = Velocity.normalized;
        }
    }
}