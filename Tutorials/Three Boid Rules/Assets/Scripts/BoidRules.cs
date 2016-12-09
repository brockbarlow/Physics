﻿namespace Assets.Scripts
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

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
            foreach (var b in _boids)
            {
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
            var pc = _boids.Where(bj => bj != b).Aggregate(Vector3.zero, (current, bj) => current + bj.transform.position);
            //Divide the Precived Center by the number of boids then return the normalized value.
            pc = pc / (_boids.Count - 1);
            return (pc - b.transform.position).normalized;
        }

        private Vector3 DispersionRule(Component b) //calculates the dispersion for boids.
        {   //Boid Rule Two: Distancing
            var c = _boids.Where(bj => (bj.transform.position - b.transform.position).magnitude <= 25*Dispersion && bj != b).Aggregate(Vector3.zero, (current, bj) => current - (bj.transform.position - b.transform.position));

            return c.normalized;
        }

        private Vector3 AlignmentRule(BoidBehavior b) //calculates the alignment for boids.
        {   //Boid Rule Three: Alignment
            var pv = _boids.Where(bj => bj != b).Aggregate(Vector3.zero, (current, bj) => current + bj.Velocity);

            pv = pv / (_boids.Count - 1);
            return (pv - b.Velocity).normalized;
        }

        private Vector3 TendTowardsPlace(Component b) //calculates the tendency for boids.
        {
            if (Tendency > 0)
            {
                return (Target.position - b.transform.position).normalized;
            }

            if (Tendency < 0 && (Target.position - b.transform.position).magnitude < TargetRange)
            {
                return (Target.position - b.transform.position).normalized;
            }

            return Vector3.zero;
        }

        private void LimitVelocity(BoidBehavior b) //limits the velocity
        {
            if (b.Velocity.magnitude > MaxSpeed)
            {
                b.Velocity = (b.Velocity / b.Velocity.magnitude) * MaxSpeed;
            }
        }

        private Vector3 WallBoundries(Component b) //sets wall boundries
        {
            var bounds = new Vector3();

            if (b.transform.position.x > Boundries)
                bounds += new Vector3(-10, 0, 0);
            else if (b.transform.position.x < -Boundries)
                bounds += new Vector3(10, 0, 0);

            if (b.transform.position.y > Boundries)
                bounds += new Vector3(0, -10, 0);
            else if (b.transform.position.y < -Boundries)
                bounds += new Vector3(0, 10, 0);

            if (b.transform.position.z > Boundries)
                bounds += new Vector3(0, 0, -10);
            else if (b.transform.position.z < -Boundries)
                bounds += new Vector3(0, 0, 10);

            return bounds;
        }

        private Vector3 CenterOfMass() //sets center of mass
        {
            var positions = _boids.Aggregate(Vector3.zero, (current, b) => current + b.transform.position);

            var centerMass = positions / _boids.Count;
            return centerMass;
        }
    }
}