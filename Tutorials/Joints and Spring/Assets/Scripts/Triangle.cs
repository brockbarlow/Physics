using UnityEngine;

public class Triangle
{
    public Vector3 surfaceNormal; //n
    public Vector3 averageVelocity; //v
    public float areaOfTriangle; //a
    public float windCoeficent = 1f;
    public Particle TP1, TP2, TP3;
    public SpringDamper SD1, SD2, SD3;

    public Triangle() { }

    public Triangle(MonoParticle one, MonoParticle two, MonoParticle three)
    {
        TP1 = one.particle;
        TP2 = two.particle;
        TP3 = three.particle;
    }

    public Triangle(Particle one, Particle two, Particle three)
    {
        TP1 = one;
        TP2 = two;
        TP3 = three;
    }

    public bool CalculateAerodynamicForce(Vector3 airVector)
    {
        Vector3 surfaceVector = ((TP1.velocity + TP2.velocity + TP3.velocity) / 3);
        averageVelocity = surfaceVector - airVector;
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