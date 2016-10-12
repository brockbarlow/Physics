using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {
    Vector3 velocity;
    Vector3 force;
    float mass;

    Vector3 displacement;
    Vector3 desiredVelocity;
    Vector3 steering;
    public Transform target;

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