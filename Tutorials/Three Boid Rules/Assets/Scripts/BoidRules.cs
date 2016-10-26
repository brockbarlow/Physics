using UnityEngine;
using System.Collections.Generic;

public class BoidRules : MonoBehaviour
{
    public GameObject prefab;
    public Transform target;         
    [Range(0, 100)]public int boidNumber;          //how many are created.
    [Range(0, 100)]public float maxBoidDistance;   //how far are they when they spawn.
    [Range(0.0f, 1.0f)]public float cohesion;      //value that will be multiplied with the returning cohesion value.
    [Range(0.0f, 1.0f)]public float dispersion;    //value that will be multiplied with the returning dispersion value.
    [Range(0.0f, 1.0f)]public float alignment;     //value that will be multiplied with the returning alignment value.

    private List<BoidBehavior> boids;              //will hold the boids.
    private Vector3 r1;                            //holds cohesion value.
    private Vector3 r2;                            //holds dispersion value.
    private Vector3 r3;                            //holds alignment value.
    private Vector3 pc;
    private Vector3 c;
    private Vector3 pv;
    private Vector3 pos;
    private BoidBehavior b;
    private GameObject temp;

    public void Awake()
    {
        boids = new List<BoidBehavior>();
        pos = Vector3.zero;
        for (int i = 0; i < boidNumber; i++)
        {
            pos.x = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.y = Random.Range(-maxBoidDistance, maxBoidDistance);
            pos.z = Random.Range(-maxBoidDistance, maxBoidDistance);

            temp = Instantiate(prefab, transform.position + pos, new Quaternion()) as GameObject;
            b = temp.GetComponent<BoidBehavior>();
            b.velocity = b.transform.position.normalized;
            b.transform.parent = transform;
            boids.Add(b);
        }
    }

    public void FixedUpdate()
    {
        foreach (BoidBehavior b in boids)
        {
            r1 = cohesionRule(b) * cohesion;
            r2 = dispersionRule(b) * dispersion;
            r3 = alignmentRule(b) * alignment;
            b.velocity += (r1 + r2 + r3);
        }
    }

    private Vector3 cohesionRule(BoidBehavior b)
    {
        pc = Vector3.zero;
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
        c = Vector3.zero;
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
        pv = Vector3.zero;
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
}