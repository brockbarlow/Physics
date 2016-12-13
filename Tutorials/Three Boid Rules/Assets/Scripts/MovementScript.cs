namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    //This script is for the moveable game object.
    //used to help demonstrate the tendency rule during runtime.
    public class MovementScript : MonoBehaviour
    {
        private Vector3 _movement;
        public float Range;

        private void Update()
        {
            _movement = Vector3.zero; //_movement equals a new Vector3

            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < Range) //when up arrow key is pressed.
            {
                _movement += Vector3.up; //shorthand for Vector3(0,1,0)
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -Range) //when down arrow key is pressed.
            {
                _movement += Vector3.down; //shorthand for Vector3(0,-1,0)
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < Range) //when right arrow key is pressed.
            {
                _movement += Vector3.right; //shorthand for Vector3(1,0,0)
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -Range) //when left arrow key is pressed.
            {
                _movement += Vector3.left; //shorthand for Vector3(-1,0,0)
            }
            //update the target game objects transform position with the modified _movement variable.
            transform.position += _movement;
        }
    }
}