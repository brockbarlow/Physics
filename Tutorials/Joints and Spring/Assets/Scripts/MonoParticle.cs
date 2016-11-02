using UnityEngine;
using System.Collections.Generic;

public class MonoParticle : MonoBehaviour
{
    [HideInInspector]public Particle particle;
    [HideInInspector]public bool anchor;
    [HideInInspector]public List<MonoParticle> neighbors;

    public void LateUpdate()
    {
        foreach (MonoParticle mp in neighbors)
        {
            Debug.DrawLine(transform.position, mp.transform.position, Color.black);
        }
    }
}