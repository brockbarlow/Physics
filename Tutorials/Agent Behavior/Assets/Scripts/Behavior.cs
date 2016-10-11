using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Behavior : MonoBehaviour
{
    //Force = mass * velocity
    //velocity = Force / mass

    public Vector3 force;
    public int mass;
    Vector3 velocity;

    void FixedUpdate()
    {
        velocity = ((force / mass) * Time.deltaTime);
    }

    void LateUpdate()
    {
        transform.position += velocity;
    }
}