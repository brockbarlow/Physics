using UnityEngine;

public class SpringDamper
{
    public Particle P1, P2;
    public float springConstant; //ks;
    public float dampingFactor; //kd;
    public float restLength; //lo;

    public SpringDamper() { } //default constructor;

    public SpringDamper(Particle one, Particle two, float ks, float kd, float lo) //custom constructor
    {                                    //spring constant, damping factor, rest length
        P1 = one;
        P2 = two;
        springConstant = ks;
        dampingFactor = kd;
        restLength = lo;
    }

    public void ComputeForce()
    {
        Vector3 dist = (P2.position - P1.position); //e*
        Vector3 e = dist / dist.magnitude;
        float p1V1D = Vector3.Dot(e, P1.velocity); //p1's 1D vector
        float p2V1D = Vector3.Dot(e, P2.velocity); //p2's 1D vector
        float springForceLinear = -springConstant * (restLength - dist.magnitude); //fs
        float dampingForceLinear = -dampingFactor * (p1V1D - p2V1D); //fd
        Vector3 springDampingForce = (springForceLinear + dampingForceLinear) * e; //fs + fd * e
        P1.AddForce(springDampingForce); //f1
        P2.AddForce(-springDampingForce); //-f1
    }
}