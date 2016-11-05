using UnityEngine;

public class MonoParticle : MonoBehaviour
{
    [HideInInspector]public Particle particle;
    [HideInInspector]public bool anchorPoints;

    public void LateUpdate()
    {
        foreach (Particle p in particle.particles)
        {
            Debug.DrawLine(transform.position, p.position, Color.black);
        }
    }
}