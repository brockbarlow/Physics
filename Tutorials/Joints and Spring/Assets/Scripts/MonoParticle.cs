using UnityEngine;

public class MonoParticle : MonoBehaviour
{
    public Particle particle;
    [HideInInspector]public bool anchorPoint;

    public void LateUpdate()
    {
        foreach (Particle p in particle.particles)
        {
            Debug.DrawLine(transform.position, p.position, Color.black);
        }
    }
}