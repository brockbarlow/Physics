using UnityEngine;

public class Triangle
{
    public Vector3 surfaceNormal; //n
    public Vector3 averageVelocity; //v
    public float areaOfTriangle; //a
    public float windCoefficient = 1f;
    public Particle TP1, TP2, TP3;
    public SpringDamper SD1, SD2, SD3;
                    //TP1P2, TP2P3, TP3P1
    public Triangle() { }

    public Triangle(MonoParticle mpOne, MonoParticle mpTwo, MonoParticle mpThree)
    {
        TP1 = mpOne.particle;
        TP2 = mpTwo.particle;
        TP3 = mpThree.particle;
    }

    public Triangle(Particle mpOne, Particle mpTwo, Particle mpThree)
    {
        TP1 = mpOne;
        TP2 = mpTwo;
        TP3 = mpThree;
    }

    public bool ComputeAerodynamicForce(Vector3 air)
    {
        Vector3 surface = ((TP1.velocity + TP2.velocity + TP3.velocity) / 3);
        averageVelocity = surface - air;
        surfaceNormal = Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)) / Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)).magnitude;

        float ao = (1f / 2f) * Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)).magnitude;
        areaOfTriangle = ao * (Vector3.Dot(averageVelocity, surfaceNormal) / averageVelocity.magnitude);
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(averageVelocity.magnitude, 2) * 1f * areaOfTriangle * surfaceNormal;
        TP1.AddForce(aeroForce / 3);
        TP2.AddForce(aeroForce / 3);
        TP3.AddForce(aeroForce / 3);

        return true;
    }
}