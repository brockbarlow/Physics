namespace Assets.Scripts
{  
    using UnityEngine;
    //This script is for the moveable game object.
    //used to help demonstrate the tendency rule during runtime.
    public class MovementScript : MonoBehaviour
    {
        [HideInInspector]public Vector3 Movement;
        public float Range;

        private void Update()
        {
            Movement = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < Range)
            {
                Movement += Vector3.up; //shorthand for Vector3(0,1,0)
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -Range)
            {
                Movement += Vector3.down; //shorthand for Vector3(0,-1,0)
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < Range)
            {
                Movement += Vector3.right; //shorthand for Vector3(1,0,0)
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -Range)
            {
                Movement += Vector3.left; //shorthand for Vector3(-1,0,0)
            }

            transform.position += Movement;
        }
    }
}