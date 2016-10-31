using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]public Vector3 movement;
    public float range;

    private void Update()
    {
        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < range)
        {
            movement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -range)
        {
            movement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < range)
        {
            movement += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -range)
        {
            movement += Vector3.left;
        }

        transform.position += movement;
    }
}