using UnityEngine;
using System.Collections.Generic;

public class Particle
{
    public Vector3 position; //r;
    public Vector3 velocity; //v;
    public Vector3 acceleration; //a; //a = (1 / m) * f;
    public Vector3 force; //f;
    public float mass; //m;
    public List<Particle> particles;

    public Particle() { } //default constructor

    public Particle(Vector3 r, Vector3 v, float m) //custom constructor
    {             //position,   velocity,   mass
        position = Vector3.zero;
        velocity = Vector3.zero;
        force = Vector3.zero;
        position = r;
        velocity = v;
        mass = (m <= 0) ? 1 : m;
    }

    public void AddForce(Vector3 f)
    {
        force += f;
    }

    public Vector3 UpdateParticle()
    {  
        acceleration = (1f / mass) * force;
        velocity += acceleration * Time.fixedDeltaTime;
        position += velocity * Time.fixedDeltaTime;
        return position;
    }
}