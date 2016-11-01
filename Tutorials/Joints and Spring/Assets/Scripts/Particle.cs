using UnityEngine;
using System;

[Serializable]
public class Particle
{
    public Vector3 m_Position; //r;
    public Vector3 m_Velocity; //v;
    public Vector3 m_Acceleration; //a; //a = (1 / m) * f;
    public Vector3 m_Momentum; //p; //p = m * v;
    public Vector3 m_Force; //f;
    public float m_mass; //m;
    public bool isKinematic;

    public Vector3 Position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

    public Vector3 Velocity
    {
        get { return m_Velocity; }
        set { m_Velocity = value; }
    }

    public Vector3 Force
    {
        get { return m_Force; }
        set { m_Force = value; }
    }

    public float Mass
    {
        get { return m_mass; }
        set { m_mass = value; }
    }

    public Particle() { } //default constructor

    public Particle(Vector3 r, Vector3 v, float m) //custom constructor
    {             //position,   velocity,   mass
        m_Position = r;
        m_Velocity = v;
        m_mass = m;
        m_Force = Vector3.zero;
        m_Momentum = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        Force += force;
    }

    public void Update()
    {  
        if (isKinematic) { return; }
        m_Acceleration = (1f / m_mass) * Force;
        m_Velocity += m_Acceleration * Time.fixedDeltaTime;
        m_Position += m_Velocity * Time.fixedDeltaTime;
    }
}