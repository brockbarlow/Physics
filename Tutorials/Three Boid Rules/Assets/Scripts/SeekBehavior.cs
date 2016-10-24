using UnityEngine;

public class SeekBehavior : MonoBehaviour
{
    MonoBoid mb;
    Vector3 desiredVelocity;
    Vector3 steering;
    public Transform target;
    public float steeringFactor;

    void Start()
    {
        mb = gameObject.GetComponent<MonoBoid>();
    }

    void FixedUpdate()
    {
        desiredVelocity = (target.position - transform.position).normalized;
        steering = (desiredVelocity - mb.agent.velocity).normalized * .1f * steeringFactor;
        mb.agent.velocity += steering / mb.agent.mass;
        if (mb.agent.velocity.magnitude > 3)
        {
            mb.agent.velocity = mb.agent.velocity.normalized;
        }
    }
}