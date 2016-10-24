using UnityEngine;

public class SeekArrive : MonoBehaviour {

    Monoboid mb;
    Vector3 desiredVelocity;
    public Transform target;
    Vector3 steering;
    public float steeringFactor;
    public float radius;

	// Use this for initialization
	void Start () {
        mb = gameObject.GetComponent<Monoboid>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float pushBackForceFactor = (target.position - transform.position).magnitude / radius;
        Vector3 pushBackForce = (target.position - transform.position).normalized * pushBackForceFactor;

        mb.agent.velocity = pushBackForce / mb.agent.mass;

        if (mb.agent.velocity.magnitude > 5)
        {
            mb.agent.velocity = mb.agent.velocity.normalized;
        }
	
	}
}
