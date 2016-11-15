using UnityEngine;
using System.Collections.Generic;

public class Particle
{
    public Vector3 position; //r;
    public Vector3 velocity; //v;
    public Vector3 acceleration; //a; //a = (1 / m) * f;
    public Vector3 force; //f;
    public float mass; //m;
    public List<Particle> particles; //list of particles

    public Particle() { } //default constructor

    public Particle(Vector3 r, Vector3 v, float m) //custom constructor //takes in position, velocity and mass values
    {             //position,   velocity,   mass
        position = Vector3.zero;
        velocity = Vector3.zero; //vector3.zero is shorthand for vector3(0,0,0)
        force = Vector3.zero;
        position = r; //position will receive r value
        velocity = v; //velocity will receive v value
        mass = (m <= 0) ? 1 : m; //if mass is equal to or less than zero, mass will equal one. if not, mass will receive m value
    }

    public void AddForce(Vector3 f) //takes in force value
    {
        force += f; //adds f value to force
    }

    public Vector3 UpdateParticle()
    {  
        acceleration = (1f / mass) * force; //formula for acceleration
        velocity += acceleration * Time.fixedDeltaTime; //adds this result to velocity
        velocity = Vector3.ClampMagnitude(velocity, velocity.magnitude); //clamp the velocity (sets it).
        position += velocity * Time.fixedDeltaTime; //adds this result to position
        //fixedDeltaTime is affected by the settings of the physics simulation. it runs at a (more or less) fixed interval and is not affected
        //by inefficiencies in code.
        return position; //return the new position
    }
}