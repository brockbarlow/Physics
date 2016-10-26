using UnityEngine;

public class SeekBehavior : MonoBehaviour
{
    private MonoBoid mb;
    private Vector3 desiredVelocity;
    private Vector3 steering;

    public Transform target;
    [HideInInspector]public float steeringFactor;

    public void Start()
    {
        mb = gameObject.GetComponent<MonoBoid>();
    }

    public void FixedUpdate()
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