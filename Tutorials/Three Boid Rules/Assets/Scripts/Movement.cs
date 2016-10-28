using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]public Vector3 movement;
    public float range;

    private void Update()
    {
        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W) && transform.position.y < range)
        {
            movement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -range)
        {
            movement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < range)
        {
            movement += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -range)
        {
            movement += Vector3.left;
        }

        transform.position += movement;
    }
}