using UnityEngine;
using Interface;

public class Monoboid : MonoBehaviour, Iboid
{
    private Agent a;
    public float mass;

    public Vector3 velocity
    {
        get { return a.velocity; }
        set { a.velocity = value; }
    }

    public Vector3 position
    {
        get { return a.position; }
        set { a.position = value; }
    }

    float Iboid.mass
    {
        get { return a.mass; }
        set { a.mass = value; }
    }

    void Start()
    {
        a = new Agent(mass);
    }

    void LateUpdate()
    {
        a.updateVelocity();
    }
}