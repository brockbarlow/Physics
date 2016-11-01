using UnityEngine;

public class MonoParticle : MonoBehaviour
{
    public Particle particle;

    public void LateUpdate()
    {
        transform.position = particle.position;
    }
}