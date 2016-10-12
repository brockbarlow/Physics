using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target : MonoBehaviour {
    public GameObject prefab;
    public int copies;
    public int location;

    void Start()
    {
        for (int i = 0; i < copies; i++)
        {
            Vector3 position = new Vector3();

            position.x = Random.Range(-location, location);
            position.y = Random.Range(-location, location);
            position.z = Random.Range(-location, location);


        }
    }
}
