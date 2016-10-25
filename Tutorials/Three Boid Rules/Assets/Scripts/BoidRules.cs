using UnityEngine;
using System.Collections.Generic;

public class BoidRules : MonoBehaviour
{
    public GameObject prefab;
    [Range(1, 100)]public int boidNumber;
    [Range(1, 100)]public float maxBoidDistance;
    public Transform target;
    [Range(0.1f, 1.0f)]public float cohesion;
    [Range(0.1f, 1.0f)]public float dispersion;
    [Range(0.0f, 1.0f)]public float alignment;

    private List<BoidBehavior> boids;

    public void Awake()
    {
        boids = new List<BoidBehavior>();
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < boidNumber; i++)
        {
            pos.x = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.y = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.z = Random.Range(-maxBoidDistance, maxBoidDistance);

            GameObject temp = Instantiate(prefab, pos, new Quaternion()) as GameObject;
            BoidBehavior bb = temp.GetComponent<BoidBehavior>();
            bb.velocity = bb.transform.position.normalized;
            bb.transform.parent = transform;
            boids.Add(bb);
        }
    }

    public void FixedUpdate()
    {
        foreach (BoidBehavior bb in boids)
        {
            Vector3 r1 = cohesionRule(bb);
            Vector3 r2 = dispersionRule(bb);
            Vector3 r3 = alignmentRule(bb);
            bb.velocity += r1 + r2 + r3;
        }
    }

    private Vector3 cohesionRule(BoidBehavior bb)
    {
        Vector3 pcj = Vector3.zero;
        foreach (BoidBehavior bj in boids)
        {
            if (bj != bb)
            {
                pcj += bj.transform.position;
            }
        }
        pcj = pcj / (boids.Count - 1);
        return (pcj - bb.transform.position).normalized * cohesion;
    }

    private Vector3 dispersionRule(BoidBehavior bb)
    {
        Vector3 c = Vector3.zero;
        foreach (BoidBehavior bj in boids)
        {
            if ((bj.transform.position - bb.transform.position).magnitude <= 100 * dispersion && bj != bb)
            {
                c -= bj.transform.position - bb.transform.position;
            }
        }
        return c.normalized;
    }

    private Vector3 alignmentRule(BoidBehavior bb)
    {
        Vector3 pvj = Vector3.zero;
        foreach (BoidBehavior bj in boids)
        {
            if (bj != bb)
            {
                pvj += bj.velocity;
            }
        }
        pvj = pvj / (boids.Count - 1);
        return (pvj - bb.velocity).normalized * alignment;
    }


}