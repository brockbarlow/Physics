using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject prefab;
    public int agentNumber;
    public int minMass;
    public int maxMass;
    public float maxDistance;
    [Range(.1f, 1.5f)]public float steeringBehavior;
    public float radius;

    void Start()
    {
        steeringBehavior = 1;
        for (int i = 0; i < agentNumber; i++)
        {
            Vector3 Pos = Vector3.zero;
            Pos.x = Random.Range(-maxDistance, maxDistance);
            Pos.y = Random.Range(-maxDistance, maxDistance);
            Pos.z = Random.Range(-maxDistance, maxDistance);

            GameObject temp = Instantiate(prefab, Pos, Quaternion.identity) as GameObject;

            SeekArrive sa = temp.GetComponent<SeekArrive>();
            sa.target = gameObject.transform;
            sa.steeringFactor = steeringBehavior;
            sa.radius = radius;

            Monoboid mb = temp.GetComponent<Monoboid>();
            mb.mass = Random.Range(minMass, maxMass);
            mb.agent.velocity = Vector3.up;
        }
    }

    void Update()
    {
        foreach (SeekArrive sb in FindObjectsOfType<SeekArrive>())
        {
            sb.target = gameObject.transform;
            sb.steeringFactor = steeringBehavior;
            sb.radius = radius;
        }
    }
}