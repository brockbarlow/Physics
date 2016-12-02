namespace Assets.Scripts
{
    using UnityEngine;

    public class Triangle
    {
        public Vector3 SurfaceNormal; //n
        public Vector3 AverageVelocity; //v
        public float AreaOfTriangle; //a
        public Particle Tp1, Tp2, Tp3; //particle triangle objects
        public SpringDamper Sd1, Sd2, Sd3; //springdamper triangle objects
                                           //TP1_P2, TP2_P3, TP3_P1
        public Triangle() { } //default constructor;

        public Triangle(MonoParticle mpOne, MonoParticle mpTwo, MonoParticle mpThree) //custom constructor
        {
            Tp1 = mpOne.Particle; //first triangle object equals monoparticle object mpone
            Tp2 = mpTwo.Particle; //second triangle object equals monoparticle object mptwo
            Tp3 = mpThree.Particle; //thrid triangel object equals monoparticle object three
        }

        public bool ComputeAerodynamicForce(Vector3 air) //takes in air value
        {
            var surface = ((Tp1.Velocity + Tp2.Velocity + Tp3.Velocity) / 3); //v surface = (v1 + v2 + v3) / 3
            AverageVelocity = surface - air; //v = v surface - v air
            SurfaceNormal = Vector3.Cross((Tp2.Position - Tp1.Position), (Tp3.Position - Tp1.Position)) / Vector3.Cross((Tp2.Position - Tp1.Position), (Tp3.Position - Tp1.Position)).magnitude; //n = (r2 - r1)x(r3 - r1) / |(r2 - r1)x(r3 - r1)|

            var ao = (1f / 2f) * Vector3.Cross((Tp2.Position - Tp1.Position), (Tp3.Position - Tp1.Position)).magnitude; //ao = 1 / 2 * |(r2 - r1)x(r3 - r1)|
            AreaOfTriangle = ao * (Vector3.Dot(AverageVelocity, SurfaceNormal) / AverageVelocity.magnitude); //a = ao * v dot n / |v|
            var aeroForce = -(1f / 2f) * 1f * Mathf.Pow(AverageVelocity.magnitude, 2) * 1f * AreaOfTriangle * SurfaceNormal;
            //faero = -(1/2) * p * |v|2 * cd * a * e //p = density of the air //cd = coefficient of drag for the object //a = cross sectional area of the object //e = unit vectro in the opposite direction of the velocity

            //for my vector3 aeroForce, instead of opposing the velocity the force pushes against the normal of the surface
            //faero = -(1/2) * p * |v|2 * cd * a * n

            Tp1.AddForce(aeroForce / 3);
            Tp2.AddForce(aeroForce / 3); //take the aeroForce value and divid it by three. add that value to the triangle particle objects
            Tp3.AddForce(aeroForce / 3);

            return true; //required for bool function
        }
    }
}