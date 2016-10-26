using UnityEngine;

public class SeekArrive : MonoBehaviour
{
    private MonoBoid mb;
    private float pushBackForceFactor;
    private Vector3 pushBackForce;

    public Transform target;
    [HideInInspector]public float steeringFactor;
    [HideInInspector]public float radius;

    public void Start()
    {
        mb = gameObject.GetComponent<MonoBoid>();
    }

    public void FixedUpdate()
    {
        pushBackForceFactor = (target.position - transform.position).magnitude / radius;
        pushBackForce = (target.position - transform.position).normalized * pushBackForceFactor;
        mb.agent.velocity = pushBackForce / mb.agent.mass;
        if (mb.agent.velocity.magnitude > 3)
        {
            mb.agent.velocity = mb.agent.velocity.normalized;
        }
    }
}