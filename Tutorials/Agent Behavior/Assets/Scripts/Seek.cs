using UnityEngine;

public class Seek : MonoBehaviour
{
    Vector3 desiredVelocity;
    Vector3 steering;
    public Transform target;
    public float steer;
    Monoboid agent;

    void Awake()
    {
        agent = gameObject.GetComponent<Monoboid>();
    }

    void FixedUpdate()
    {
        desiredVelocity = (target.position - transform.position).normalized;
        steering = (desiredVelocity - agent.velocity).normalized * steer;
        agent.velocity += steering / agent.mass;
        if (agent.velocity.magnitude > 5)
        {
            agent.velocity = agent.velocity.normalized;
        }
    }
}