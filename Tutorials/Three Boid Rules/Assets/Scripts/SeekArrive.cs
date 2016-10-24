using UnityEngine;

public class SeekArrive : MonoBehaviour
{
    private MonoBoid mb;
    private Vector3 desiredVelocity;
    private Vector3 steering;

    public Transform target;
    public float steeringFactor;
    public float radius;

    public void Start()
    {
        mb = gameObject.GetComponent<MonoBoid>();
    }

    public void FixedUpdate()
    {
        float pushBackForceFactor = (target.position - transform.position).magnitude / radius;
        Vector3 pushBackForce = (target.position - transform.position).normalized * pushBackForceFactor;
        mb.agent.velocity = pushBackForce / mb.agent.mass;
        if (mb.agent.velocity.magnitude > 3)
        {
            mb.agent.velocity = mb.agent.velocity.normalized;
        }
    }
}