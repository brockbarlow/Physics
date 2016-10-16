using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public GameObject prefab;
    public int agentCount;
    public int minMass;
    public int maxMass;
    public float maxDistance;
    [Range(.1f, 1.5f)]public float steerBehavior;

    void Awake()
    {
        steerBehavior = 1;
        for (int i = 0; i < agentCount; i++)
        {
            Vector3 Position = new Vector3();
            Position.x = Random.Range(-maxDistance, maxDistance);
            Position.y = Random.Range(-maxDistance, maxDistance);
            Position.z = Random.Range(-maxDistance, maxDistance);

            GameObject temp = Instantiate(prefab, Position, Quaternion.identity) as GameObject;

            Seek s = temp.GetComponent<Seek>();
            s.target = gameObject.transform;
            s.steer = steerBehavior;

            Monoboid mb = temp.GetComponent<Monoboid>();
            mb.mass = Random.Range(minMass, maxMass);
            mb.velocity = new Vector3(0, 1, 0);
        }
    }

    void Update()
    {
        foreach (Seek s in FindObjectsOfType<Seek>())
        {
            s.target = gameObject.transform;
            s.steer = steerBehavior;
        }
    }
}