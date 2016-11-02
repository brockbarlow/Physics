using UnityEngine;
using System;

[Serializable]
public class SpringDamper
{
    public Particle p1, p2;
    public float springConstant; //ks;
    public float dampingFactor; //kd;
    public float restLength; //lo;

    public SpringDamper() { } //default constructor;

    public SpringDamper(Particle P1, Particle P2, float ks, float kd, float lo) //custom constructor
    {                                    //spring constant, damping factor, rest length
        p1 = P1;
        p2 = P2;
        springConstant = ks;
        dampingFactor = kd;
        restLength = lo;
    }

    public void ComputeForce()
    {
        Vector3 dist = (p2.position - p1.position);
        Vector3 e = dist / dist.magnitude;
        float p1V1D = Vector3.Dot(e, p1.velocity); 
        float p2V1D = Vector3.Dot(e, p2.velocity); 
        float springForceLinear = -springConstant * (restLength - dist.magnitude);
        float dampingForceLinear = -dampingFactor * (p1V1D - p2V1D);
        Vector3 springForce = (springForceLinear + dampingForceLinear) * e;
        p1.AddForce(springForce);
        p2.AddForce(-springForce);
    }
}