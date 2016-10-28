using UnityEngine;
using System.Collections.Generic;

public class BoidRules : MonoBehaviour
{
    public GameObject prefab;
    public Transform target;

    public float targetRange;
    public float maxSpeed;

    [Range(0, 100)]public int boidNumber;          
    [Range(0, 100)]public float maxBoidDistance;   

    [Range(0.0f, 1.0f)]public float cohesion;      
    [Range(0.0f, 1.0f)]public float dispersion;    
    [Range(0.0f, 1.0f)]public float alignment;     
    [Range(-1.0f, 1.0f)]public float tendency;

    private List<BoidBehavior> boids;              

    private void Awake()
    {
        boids = new List<BoidBehavior>();
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < boidNumber; i++)
        {
            pos.x = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.y = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.z = Random.Range(-maxBoidDistance, maxBoidDistance);

            GameObject temp = Instantiate(prefab, transform.position + pos, new Quaternion()) as GameObject;
            BoidBehavior b = temp.GetComponent<BoidBehavior>();

            b.velocity = b.transform.position.normalized;
            b.transform.parent = transform;
            boids.Add(b);
        }
    }

    private void FixedUpdate()
    {
        foreach (BoidBehavior b in boids)
        {
            Vector3 r1 = cohesionRule(b) * cohesion;
            Vector3 r2 = dispersionRule(b) * dispersion;
            Vector3 r3 = alignmentRule(b) * alignment;
            Vector3 tendTowards = tendTowardsPlace(b) * tendency;

            b.velocity += (r1 + r2 + r3 + tendTowards) / b.mass;
            LimitVelocity(b);
        }
    }

    private Vector3 cohesionRule(BoidBehavior b)
    {
        Vector3 pc = Vector3.zero;

        foreach (BoidBehavior bj in boids)
        {
            if (bj != b)
            {
                pc += bj.transform.position; 
            }
        }

        pc = pc / (boids.Count - 1);
        return (pc - b.transform.position).normalized; 
    }

    private Vector3 dispersionRule(BoidBehavior b)
    {
        Vector3 c = Vector3.zero;

        foreach (BoidBehavior bj in boids)
        {
            if ((bj.transform.position - b.transform.position).magnitude <= 20  && bj != b) 
            {
                c -= (bj.transform.position - b.transform.position); 
            }
        }

        return c.normalized;
    }

    private Vector3 alignmentRule(BoidBehavior b)
    {
        Vector3 pv = Vector3.zero;

        foreach (BoidBehavior bj in boids)
        {
            if (bj != b)
            {
                pv += bj.velocity;
            }
        }

        pv = pv / (boids.Count - 1);
        return (pv - b.velocity).normalized;
    }

    private Vector3 tendTowardsPlace(BoidBehavior b)
    {
        if (tendency > 0)
        {
            return (target.position - b.transform.position).normalized;
        }

        else if (tendency < 0 && (target.position - b.transform.position).magnitude < targetRange)
        {
            return (target.position - b.transform.position).normalized;
        }

        else
        {
            return Vector3.zero;
        }
    }

    private void LimitVelocity(BoidBehavior b)
    {
        if (b.velocity.magnitude > maxSpeed)
        {
            b.velocity = (b.velocity / b.velocity.magnitude) * maxSpeed;
        }
    }
}