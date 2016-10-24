using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidBehavior : MonoBehaviour
{
    private Vector3 velocity;
    private List<BoidBehavior> boids;

    public GameObject prefab;
    public int boidNumber;
    public float maxDistance;
    public Transform target;
    [Range(0.1f, 1.5f)]public float cohesion;
    [Range(0.1f, 1.5f)]public float dispersion;
    [Range(0.1f, 1.5f)]public float alignment;

    public void Awake()
    {
        boids = new List<BoidBehavior>();
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < boidNumber; i++)
        {
            pos.x = Random.Range(-maxDistance, maxDistance);
            pos.y = Random.Range(-maxDistance, maxDistance);
            pos.z = Random.Range(-maxDistance, maxDistance);

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
            Vector3 r1 = Rule1(bb);
            Vector3 r2 = Rule2(bb);
            Vector3 r3 = Rule3(bb);
            bb.velocity += r1 + r2 + r3;
        }
    }

    private Vector3 Rule1(BoidBehavior bb)
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

    private Vector3 Rule2(BoidBehavior bb)
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

    private Vector3 Rule3(BoidBehavior bb)
    {

    }

    public void LateUpdate()
    {
        transform.position += velocity.normalized;
        transform.forward = velocity.normalized;
    }
}