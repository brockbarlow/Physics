namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    using System.Collections.Generic;

    public class BoidRules : MonoBehaviour
    {
        public GameObject Prefab;
        public Transform Target;
        public Transform Center;

        public float TargetRange;
        public float Boundries;
        public float MaxSpeed;
        public float MinMass;
        public float MaxMass;

        [Range(0, 100)]public int BoidNumber;
        [Range(0, 100)]public float MaxBoidDistance;

        [Range(0.0f, 1.0f)]public float Cohesion;
        [Range(0.0f, 1.0f)]public float Dispersion;
        [Range(0.0f, 1.0f)]public float Alignment;
        [Range(-1.0f, 1.0f)]public float Tendency;

        private List<BoidBehavior> _boids;

        private void Awake() //spawn the boids and add them to the list.
        {
            MaxSpeed = (MaxSpeed <= 0) ? 1 : MaxSpeed;
            TargetRange = (TargetRange <= 0) ? 20 : TargetRange;

            _boids = new List<BoidBehavior>();
            var pos = Vector3.zero;
            for (var i = 0; i < BoidNumber; i++)
            {
                pos.x = Random.Range(-MaxBoidDistance, MaxBoidDistance);
                pos.y = Random.Range(-MaxBoidDistance, MaxBoidDistance);
                pos.z = Random.Range(-MaxBoidDistance, MaxBoidDistance);

                var temp = Instantiate(Prefab, transform.position + pos, new Quaternion()) as GameObject;

                if (temp == null) continue;
                var b = temp.GetComponent<BoidBehavior>();

                b.Velocity = b.transform.position.normalized;
                b.transform.parent = transform;
                b.Mass = Random.Range(MinMass, MaxMass);
                _boids.Add(b);
            }
        }

        private void FixedUpdate() //math results are applied to the boids.
        {
            foreach (var b in _boids) //for each b in boids...
            { //apply values and call functions
                var r1 = CohesionRule(b) * Cohesion;
                var r2 = DispersionRule(b);
                var r3 = AlignmentRule(b) * Alignment;
                var walls = WallBoundries(b);
                var tendTowards = TendTowardsPlace(b) * Tendency;

                b.Velocity += ((r1 + r2 + r3 + walls + tendTowards) / b.Mass);
                LimitVelocity(b);
            }
            Center.position = CenterOfMass();
        }

        private Vector3 CohesionRule(Component b) //calculates the cohesion for boids.
        {   //Boid Rule One: Center of Mass
            var pc = Vector3.zero; //pc variable equals new vector3 vector

            foreach (var bj in _boids) //for each bj in boids...
            {
                if (bj != b) //if bj does not equal b...
                {
                    pc += bj.transform.position; //add the transform position of bj to pc
                }
            }
            //Divide the Precived Center by the number of boids then return the normalized value.
            pc = pc / (_boids.Count - 1);
            return (pc - b.transform.position).normalized;
        }

        private Vector3 DispersionRule(Component b) //calculates the dispersion for boids.
        {   //Boid Rule Two: Distancing
            var c = Vector3.zero; //c variable equals new vector3 vector

            foreach (var bj in _boids) //for each bj variable in boids...
            {   //if the magnitude of bj's transform position minus the transform position of b is less than or equal to 25 times the
                //Dispersion value AND bj's value does not equal b's value...
                if ((bj.transform.position - b.transform.position).magnitude <= 25 * Dispersion && bj != b)
                {   //c equals the result of bj's transform position minus the transform position of b.
                    c -= (bj.transform.position - b.transform.position);
                }
            }

            return c.normalized; //return the normalized value of c
        }

        private Vector3 AlignmentRule(BoidBehavior b) //calculates the alignment for boids.
        {   //Boid Rule Three: Alignment
            var pv = Vector3.zero; //pv variable equals a new vector3 vector
            
            foreach (var bj in _boids) //for each bj variable in boids...
            {
                if (bj != b) //if the bj value does not equal b...
                {
                    pv += bj.Velocity; //add the velocity of bj to pv
                }
            }

            pv = pv / (_boids.Count - 1);  //divid pv by the number of boids.
            return (pv - b.Velocity).normalized; //return the normalized value of pv minus the velocity of b
        }

        private Vector3 TendTowardsPlace(Component b) //calculates the tendency for boids.
        {
            if (Tendency > 0) //if tendency is greater than zero, take the target's position and subtract it with b's transform
            {                 //position, then normalize that value. return this value.
                return (Target.position - b.transform.position).normalized;
            }
            // if tendency is less than zero AND the magnitude value of the target position minus b's transform position is less than
            // the TargetRange value...
            if (Tendency < 0 && (Target.position - b.transform.position).magnitude < TargetRange)
            {                 //return the same normalized value thats in the first if statement.
                return (Target.position - b.transform.position).normalized;
            }

            return Vector3.zero; //return a new vector3 vector if the if statements are not triggered.
        }

        private void LimitVelocity(BoidBehavior b) //limits the velocity
        {
            if (b.Velocity.magnitude > MaxSpeed) //if the magnitude of b's velocity is greater than the MaxSpeed value...
            {                                    //take b's velocity and divid it by the magnitude of b's velocity then multiple by MaxSpeed
                b.Velocity = (b.Velocity / b.Velocity.magnitude) * MaxSpeed;
            }
        }

        private Vector3 WallBoundries(Component b) //sets wall boundries
        {
            var bounds = Vector3.zero; //bounds variable equals new Vector3 vector

            if (b.transform.position.x > Boundries)         //if the x position transform of b is greater than boundries,
                bounds += new Vector3(-10, 0, 0);           //bounds will equal a new vector3 with a -10 x value.
            else if (b.transform.position.x < -Boundries)   //else, if the x position transform of b is less than boundries,
                bounds += new Vector3(10, 0, 0);            //bounds will equal a new vector3 with a +10 x value.

            if (b.transform.position.y > Boundries)         //if the y position transform of b is greater than boundries,
                bounds += new Vector3(0, -10, 0);           //bounds will equal a new vector3 with a -10 y value.
            else if (b.transform.position.y < -Boundries)   //else, if the y position transform of b is less than boundries,
                bounds += new Vector3(0, 10, 0);            //bounds will equal a new vector3 with a +10 x value.

            if (b.transform.position.z > Boundries)         //if the z position transform of b is greater than boundries,
                bounds += new Vector3(0, 0, -10);           //bounds will equal a new vector3 with a -10 z value.
            else if (b.transform.position.z < -Boundries)   //else, if the z position transform of b is less than boundries,
                bounds += new Vector3(0, 0, 10);            //bounds will equal a new vector3 with a +10 z value.

            return bounds; //return the new value in bounds variable.
        }

        private Vector3 CenterOfMass() //sets center of mass
        {
            var positions = Vector3.zero; //position variable equals new Vector3 vector

            foreach (var b in _boids)
            {   //for each b in boids, add their transform position to the positions variable
                positions += b.transform.position;
            }
            //centerMass equals the positions value divided by the number of voids. return the center of mass variable.
            var centerMass = positions / _boids.Count;
            return centerMass;
        }
    }
}