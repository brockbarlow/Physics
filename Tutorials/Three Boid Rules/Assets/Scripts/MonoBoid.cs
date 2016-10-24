using UnityEngine;

public class MonoBoid : MonoBehaviour
{
    public Agent agent;
    public float mass;

    void Awake()
    {
        agent = new Agent(mass);
    }

    void LateUpdate()
    {
        agent.UpdateVelocity();
        transform.position = agent.position;
    }
}