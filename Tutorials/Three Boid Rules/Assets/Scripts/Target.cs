using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public GameObject prefab;
    public int agentNumber;
    public int minMass;
    public int maxMass;
    public float maxDistance;
    public float radius;
    [Range(.1f, 1.5f)]public float steeringBehavior;
    public Vector3 pos;

    void Awake()
    {
        for (int i = 0; i < agentNumber; i++)
        {
            pos.x = Random.Range(-maxDistance, maxDistance);
            pos.y = Random.Range(-maxDistance, maxDistance);
            pos.z = Random.Range(-maxDistance, maxDistance);

            GameObject temp = Instantiate(prefab, pos, new Quaternion()) as GameObject;

            if (temp.GetComponent<SeekBehavior>() != null)
            {
                SeekBehavior sa = temp.GetComponent<SeekBehavior>();
                sa.target = gameObject.transform;
                sa.steeringFactor = steeringBehavior;
            }
            if (temp.GetComponent<SeekArrive>() != null)
            {
                SeekArrive sa = temp.GetComponent<SeekArrive>();
                sa.target = gameObject.transform;
                sa.steeringFactor = steeringBehavior;
                sa.radius = radius;
            }

            MonoBoid mb = temp.GetComponent<MonoBoid>();
            mb.agent.position = pos;
            mb.mass = Random.Range(minMass, maxMass);
            mb.agent.velocity = Vector3.up;
        }
    }

    void Update()
    {
        foreach (SeekArrive sb in FindObjectOfType<SeekArrive>())
        {
            sb.target = gameObject.transform;
            sb.steeringFactor = steeringBehavior;
            sb.radius = radius;
        }
        foreach (SeekBehavior sb in FindObjectOfType<SeekBehavior>())
        {
            sb.target = gameObject.transform;
            sb.steeringFactor = steeringBehavior;
        }
    }
}