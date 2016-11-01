using UnityEngine;

public class Particle
{
    public Vector3 position; //r;
    public Vector3 velocity; //v;
    public float mass; //m;
    public Vector3 steering;

    public Particle() //default constructor
    {

    }

    public Particle(Vector3 r, Vector3 v, float m) //custom constructor
    {              //position,  velocity,  mass
        position = Vector3.zero;
        velocity = Vector3.zero;
        mass = (m == 0) ? 1 : m;
        steering = Vector3.zero;
        position = r;
        velocity = v;
    }

    public Vector3 UpdateParticle(float g)
    {                            //gravity
        steering += new Vector3(0, -9.8f * g, 0);
        velocity += steering / mass;
        if (velocity.magnitude > 3)
        {
            velocity = velocity.normalized;
        }
        position += velocity;
        return position;
    }
}