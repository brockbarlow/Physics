namespace Assets.Scripts
{
    using UnityEngine;

    public class MovementScript : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 Movement;
        public float Range;

        private void Update()
        {
            Movement = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < Range)
            {
                Movement += Vector3.up;
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -Range)
            {
                Movement += Vector3.down;
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < Range)
            {
                Movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -Range)
            {
                Movement += Vector3.left;
            }

            transform.position += Movement;
        }
    }
}