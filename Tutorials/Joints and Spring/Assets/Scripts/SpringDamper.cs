using UnityEngine;

public class SpringDamper
{
    public float springConstant; //ks;
    public float dampingFactor; //kd;
    public float restLength; //lo;
    public Particle P1, P2; 
    public Vector3 springForce1; //f(sd)e;
    public Vector3 springForce2; //-f1;

    public SpringDamper() //default constructor;
    {

    }

    public SpringDamper(Particle one, Particle two, float ks, float kd, float lo) //custom constructor
    {
        P1 = one;
        P2 = two;
        springConstant = ks;
        dampingFactor = kd;
        restLength = lo;
    }

    public void ComputeForce()
    {
        Vector3 eStar = (P2.position - P1.position); //e*
        float l = eStar.magnitude; //|e|
        Vector3 e = (eStar / l);

        float P1vec1D = Vector3.Dot(e, P1.velocity); //finding 1D vector;
        float P2vec1D = Vector3.Dot(e, P2.velocity); //finding 1D vector;

        float springForceLinear = -(springConstant) * (restLength - l);
        float springDamper = dampingFactor * (P1vec1D - P2vec1D);
        float springDamperForce = springForceLinear - springDamper;

        springForce1 = springDamperForce * e;
        springForce2 = -(springForce1);

        P1.steering += springForce1;
        P2.steering += springForce2;
    }
}