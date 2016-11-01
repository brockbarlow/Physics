using UnityEngine;

public class MonoParticle : MonoBehaviour
{
    public Particle particle;
    public bool anchor;

    public void LateUpdate()
    {
        if (anchor == false)
        {
            transform.position = particle.position;
        }
    }
}