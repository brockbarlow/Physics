namespace Assets.Scripts
{
    using UnityEngine;

    public class BoidBehavior : MonoBehaviour
    {
        [HideInInspector]public Vector3 Velocity;
        [HideInInspector]public float Mass;

        public void LateUpdate()
        {
            transform.position += Velocity;
        }
    }
}