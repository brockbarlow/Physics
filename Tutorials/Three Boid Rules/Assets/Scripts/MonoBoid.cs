using UnityEngine;

public class MonoBoid : MonoBehaviour
{
    public Agent agent;
    public float mass;

    public void Awake()
    {
        agent = new Agent(mass);
    }

    public void LateUpdate()
    {
        agent.UpdateVelocity();
        transform.position = agent.position;
    }
}