using UnityEngine;
using Interface;

public class Agent : Iboid
{
    private Vector3 v_velocity;
    private Vector3 v_position;
    private float f_mass;

    public Vector3 velocity
    {
        get { return v_velocity; }
        set { v_velocity = value; }
    }

    public Vector3 position
    {
        get { return v_position; }
        set { v_position = value; }
    }

    public float mass
    {
        get { return f_mass; }
        set { f_mass = value; }
    }

    public Agent(float m)
    {
        velocity = new Vector3();
        position = new Vector3();
        mass = (m <= 0) ? 1 : m;
    }

    public void updateVelocity()
    {
        position += velocity;
    }
}