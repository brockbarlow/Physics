namespace Assets.Scripts
{
    using UnityEngine;

    public class SpringDamper
    {
        public Particle P1, P2; //particle objects one and two
        public float SpringConstant; //ks;
        public float DampingFactor; //kd;
        public float RestLength; //lo;

        public SpringDamper() //default constructor;
        {
            SpringConstant = DampingFactor = RestLength = 0;
        } 

        public SpringDamper(Particle one, Particle two, float ks, float kd, float lo) //custom constructor //takes in particle object one, particle two, spring constant, damping factor and rest length values.
        {                                    //spring constant, damping factor, rest length
            P1 = one; //P1 will receive one value
            P2 = two; //P2 will receive two value
            SpringConstant = ks; //springConstant will receive ks value
            DampingFactor = kd; //dampingFactor will receive kd value
            RestLength = lo; //restLength will receive lo value
        }

        public void ComputeForce()
        {
            var dist = (P2.Position - P1.Position); //e* //e* = r2 - r1
            var e = dist / dist.magnitude; //e = e* / l  //l = e*.magnitude
            var p1V1D = Vector3.Dot(e, P1.Velocity); //p1's 1D vector  //v1 = e dot v1
            var p2V1D = Vector3.Dot(e, P2.Velocity); //p2's 1D vector  //v2 = e dot v2
            var springForceLinear = -SpringConstant * (RestLength - dist.magnitude); //fs //fs = -ks(lo - l)
            var dampingForceLinear = -DampingFactor * (p1V1D - p2V1D); //fd //fd = -kd(v1 - v2)
            var springDampingForce = (springForceLinear + dampingForceLinear) * e; //fsd = -ks(lo - l) -kd(v1 - v2) //times e for f1
            P1.AddForce(springDampingForce); //f1 //positive value
            P2.AddForce(-springDampingForce); //f2 //-f1
        }

        public bool ThreadTearing(float tf) //takes in tear factor value
        {
            if ((P2.Position - P1.Position).magnitude > RestLength * tf) //if magnitude value is greater than the value of rest length times tear factor, do the following. if not, return false.
            {
                if (P2.Particles.Contains(P1)) //if particle object two's particles list contains a particle object one, remove it
                {
                    P2.Particles.Remove(P1);
                }

                if (P1.Particles.Contains(P2)) //if particle object one's particles list contains a particel object two, remove it
                {
                    P1.Particles.Remove(P2);
                }
                return true;
            }
            return false;
        }
    }
}