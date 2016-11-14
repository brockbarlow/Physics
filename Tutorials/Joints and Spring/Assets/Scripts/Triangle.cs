using UnityEngine;

public class Triangle
{
    public Vector3 surfaceNormal; //n
    public Vector3 averageVelocity; //v
    public float areaOfTriangle; //a
    public Particle TP1, TP2, TP3; //particle triangle objects
    public SpringDamper SD1, SD2, SD3; //springdamper triangle objects
                 //TP1_P2, TP2_P3, TP3_P1
    public Triangle() { } //default constructor;

    public Triangle(MonoParticle mpOne, MonoParticle mpTwo, MonoParticle mpThree) //custom constructor one
    {
        TP1 = mpOne.particle; //first triangle object equals monoparticle object mpone
        TP2 = mpTwo.particle; //second triangle object equals monoparticle object mptwo
        TP3 = mpThree.particle; //thrid triangel object equals monoparticle object three
    }

    public Triangle(Particle mpOne, Particle mpTwo, Particle mpThree) //custom constructor two
    {
        TP1 = mpOne; //first triangle object equals particle object mpone
        TP2 = mpTwo; //second triangle object equals particle object mptwo
        TP3 = mpThree; //thrid triangle object equals particle object mp three
    }

    public bool ComputeAerodynamicForce(Vector3 air) //takes in air value
    {
        Vector3 surface = ((TP1.velocity + TP2.velocity + TP3.velocity) / 3); //v surface = (v1 + v2 + v3) / 3
        averageVelocity = surface - air; //v = v surface - v air
        surfaceNormal = Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)) / Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)).magnitude; //n = (r2 - r1)x(r3 - r1) / |(r2 - r1)x(r3 - r1)|

        float ao = (1f / 2f) * Vector3.Cross((TP2.position - TP1.position), (TP3.position - TP1.position)).magnitude; //ao = 1 / 2 * |(r2 - r1)x(r3 - r1)|
        areaOfTriangle = ao * (Vector3.Dot(averageVelocity, surfaceNormal) / averageVelocity.magnitude); //a = ao * v dot n / |v|
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(averageVelocity.magnitude, 2) * 1f * areaOfTriangle * surfaceNormal;
        //faero = -(1/2) * p * |v|2 * cd * a * e //p = density of the air //cd = coefficient of drag for the object //a = cross sectional area of the object //e = unit vectro in the opposite direction of the velocity

        //for my vector3 aeroForce, instead of opposing the velocity the force pushes against the normal of the surface
        //faero = -(1/2) * p * |v|2 * cd * a * n

        TP1.AddForce(aeroForce / 3);
        TP2.AddForce(aeroForce / 3); //take the aeroForce value and divid it by three. add that value to the triangle particle objects
        TP3.AddForce(aeroForce / 3);

        return true; //required for bool function
    }
}