using UnityEngine;

public class MonoParticle : MonoBehaviour
{
    public Particle particle;
    public bool anchorPoint;

    //for editer view only. disable when project is done.
    //public void LateUpdate()
    //{
    //    foreach (Particle p in particle.particles)
    //    {
    //        Debug.DrawLine(transform.position, p.position, Color.black);
    //    }
    //}
}