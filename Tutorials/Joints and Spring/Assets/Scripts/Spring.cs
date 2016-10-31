using UnityEngine;
using System.Collections.Generic;

public class Spring
{
    public float SpringConstant; //ks;
    public float DampingFactor; //kd;
    public float RestLength; //lo;
    Particle P1, P2; //particle objects;

    public float SpringForceLinear; //-ks(lo - l);
    public float SpringDamper; //-kd(v1 - v2);
    public float forceSD; //-ks(lo - l) - -kd(v1 - v2); //SpringForceLinear - SpringDamper;
    public Vector3 SpringForce1; //forceSD * e;
    public Vector3 SpringForce2; //-f1;

    private List<Particle> particles;

    public void ComputeForce()
    {
        //foreach particle, apply force;
        //fGravity = m * g0;
        //g0 = [0, -9.8, 0] * (m / (s * s));

        //foreach spring-damper, compute & apply force;
        Vector3 eStar = P2.r - P1.r;
        float l = eStar.magnitude; //|e|;
        Vector3 e = (eStar / l).normalized;

        P1.v1D = Vector3.Dot(e, P1.v); //finding 1D vector;
        P2.v1D = Vector3.Dot(e, P2.v); //finding 1D vector;

        SpringForceLinear = -SpringConstant * (RestLength - l);
        SpringDamper = -DampingFactor * (P1.v1D - P2.v1D);
        forceSD = SpringForceLinear - SpringDamper;
        SpringForce1 = forceSD * e;
        SpringForce2 = -SpringForce1;
    }
}