using UnityEngine;

public class Seek : MonoBehaviour
{
    Vector3 desiredVelocity;
    Vector3 steering;
    public Transform target;
    public float steeringFactor;
    Monoboid mb;

    void Start()
    {
        mb = gameObject.GetComponent<Monoboid>();
    }

    void FixedUpdate()
    {
        desiredVelocity = (target.position - transform.position).normalized;
        steering = (desiredVelocity - mb.agent.velocity).normalized * steeringFactor;
        mb.agent.velocity += steering / mb.agent.mass;
        if (mb.agent.velocity.magnitude > 5)
        {
            mb.agent.velocity = mb.agent.velocity.normalized;
        }
    }
}