namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    using System.Collections.Generic;

    public class Particle
    {
        public Vector3 Position; //r;
        public Vector3 Velocity; //v;
        public Vector3 Acceleration; //a; //a = (1 / m) * f;
        public Vector3 Force; //f;
        public float Mass; //m;
        public List<Particle> Particles; //list of particles

        public Particle() //default constructor
        {
            Position = Vector3.zero;
            Velocity = Vector3.zero; //vector3.zero is shorthand for vector3(0,0,0)
            Force = Vector3.zero;
        } 

        public Particle(Vector3 r, Vector3 v, float m) //custom constructor //takes in position, velocity and mass values
        {             //position,   velocity,   mass
            Position = r; //position will receive r value
            Velocity = v; //velocity will receive v value
            Mass = (m <= 0) ? 1 : m; //if mass is equal to or less than zero, mass will equal one. if not, mass will receive m value
        }

        public void AddForce(Vector3 f) //takes in force value
        {
            Force += f; //adds f value to force
        }

        public Vector3 UpdateParticle()
        {
            Acceleration = (1f / Mass) * Force; //formula for acceleration
            Velocity += Acceleration * Time.fixedDeltaTime; //adds this result to velocity
            Velocity = Vector3.ClampMagnitude(Velocity, Velocity.magnitude); //clamp the velocity (sets it).
            Position += Velocity * Time.fixedDeltaTime; //adds this result to position
                                                        //fixedDeltaTime is affected by the settings of the physics simulation. it runs at a (more or less)
                                                        //fixed interval and is not affected by inefficiencies in code.
            return Position; //return the new position
        }
    }
}