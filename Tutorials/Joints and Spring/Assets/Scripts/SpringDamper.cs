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
        Vector3 eStar = (p2.Position - p1.Position);
        Vector3 e = eStar.normalized;

        float p1V1D = Vector3.Dot(e, p1.Velocity); 
        float p2V1D = Vector3.Dot(e, p2.Velocity); 

        float springForceLinear = -springConstant * (restLength - eStar.magnitude);
        float dampingForceLinear = -dampingFactor * (p1V1D - p2V1D);

        Vector3 springForce = (springForceLinear + dampingForceLinear) * e;

        p1.AddForce(springForce);
        p2.AddForce(-springForce);
    }
}