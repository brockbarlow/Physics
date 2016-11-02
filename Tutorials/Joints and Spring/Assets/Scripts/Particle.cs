using UnityEngine;
using System;

[Serializable]
public class Particle
{
    [HideInInspector]public Vector3 position; //r;
    [HideInInspector]public Vector3 velocity; //v;
    [HideInInspector]public Vector3 acceleration; //a; //a = (1 / m) * f;
    [HideInInspector]public Vector3 force; //f;
    [HideInInspector]public float mass; //m;

    public Particle() { } //default constructor

    public Particle(Vector3 r, Vector3 v, float m) //custom constructor
    {             //position,   velocity,   mass
        position = Vector3.zero;
        position = r;
        velocity = Vector3.zero;
        velocity = v;
        mass = m;
        force = Vector3.zero;
    }

    public void AddForce(Vector3 f)
    {
        force += f;
    }

    public Vector3 Update()
    {  
        acceleration = (1f / mass) * force;
        velocity += acceleration * Time.fixedDeltaTime;
        position += velocity * Time.fixedDeltaTime;
        return position;
    }
}