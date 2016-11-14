using UnityEngine;

public class SpringDamper
{
    public Particle P1, P2; //particle objects one and two
    public float springConstant; //ks;
    public float dampingFactor; //kd;
    public float restLength; //lo;

    public SpringDamper() { } //default constructor;

    public SpringDamper(Particle one, Particle two, float ks, float kd, float lo) //custom constructor //takes in particle object one, particle two, spring constant, damping factor and rest length values.
    {                                    //spring constant, damping factor, rest length
        P1 = one; //P1 will receive one value
        P2 = two; //P2 will receive two value
        springConstant = ks; //springConstant will receive ks value
        dampingFactor = kd; //dampingFactor will receive kd value
        restLength = lo; //restLength will receive lo value
    }

    public void ComputeForce()
    {
        Vector3 dist = (P2.position - P1.position); //e* //e* = r2 - r1
        Vector3 e = dist / dist.magnitude; //e = e* / l  //l = e*.magnitude
        float p1V1D = Vector3.Dot(e, P1.velocity); //p1's 1D vector  //v1 = e dot v1
        float p2V1D = Vector3.Dot(e, P2.velocity); //p2's 1D vector  //v2 = e dot v2
        float springForceLinear = -springConstant * (restLength - dist.magnitude); //fs //fs = -ks(lo - l)
        float dampingForceLinear = -dampingFactor * (p1V1D - p2V1D); //fd //fd = -kd(v1 - v2)
        Vector3 springDampingForce = (springForceLinear + dampingForceLinear) * e; //fsd = -ks(lo - l) -kd(v1 - v2) //times e for f1
        P1.AddForce(springDampingForce); //f1 //positive value
        P2.AddForce(-springDampingForce); //f2 //-f1
    }

    public bool threadTearing(float tf) //takes in tear factor value
    {
        if ((P2.position - P1.position).magnitude > restLength * tf) //if magnitude value is greater than the value of rest length times tear factor, do the following. if not, return false.
        {
            if (P2.particles.Contains(P1)) //if particle object two's particles list contains a particle object one, remove it
            {
                P2.particles.Remove(P1);
            }

            if (P1.particles.Contains(P2)) //if particle object one's particles list contains a particel object two, remove it
            {
                P1.particles.Remove(P2);
            }
            return true;
        }
        return false;
    }
}