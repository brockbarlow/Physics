using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Behavior : MonoBehaviour
{
    //Force = mass * velocity
    //velocity = Force / mass

    Vector3 force;
    float mass;
    Vector3 velocity;

    Vector3 steering;
    Vector3 desiredVelocity;
    Vector3 displacement;
    public Transform target;

    void Start()
    {
        force.x = Random.Range(10, 75);
        force.y = Random.Range(10, 75);
        force.z = Random.Range(10, 75);
        mass = Random.Range(10, 75);
    }

    void FixedUpdate()
    {
        displacement = target.position - transform.position;
        desiredVelocity = displacement.normalized;
        steering = (desiredVelocity - velocity).normalized / mass;
        velocity += steering;

        if (velocity.magnitude > 5)
        {
            velocity = velocity.normalized;
        }
    }

    void LateUpdate()
    {
        transform.position += velocity;
    }
}