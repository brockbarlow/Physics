using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject prefab;
    [Range(1, 100)]public int agentNumber;
    [Range(1, 10)]public int minMass;
    [Range(1, 10)]public int maxMass;
    [Range(1, 100)]public float maxDistance;
    [Range(1, 50)]public float radius;
    [Range(0.1f, 1.5f)]public float steeringBehavior;

    private Vector3 pos;

    public void Awake()
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

    public void Update()
    {
        foreach (SeekArrive sb in FindObjectsOfType<SeekArrive>())
        {
            sb.target = gameObject.transform;
            sb.steeringFactor = steeringBehavior;
            sb.radius = radius;
        }
        foreach (SeekBehavior sb in FindObjectsOfType<SeekBehavior>())
        {
            sb.target = gameObject.transform;
            sb.steeringFactor = steeringBehavior;
        }
    }
}