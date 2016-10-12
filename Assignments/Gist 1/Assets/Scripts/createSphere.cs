/*gist1: refactor your c++ generate halfcircle and generate sphere functions to c# then execute them in unityengine*/

using UnityEngine;
using System.Collections.Generic;

public class createSphere : MonoBehaviour {

    //radius = 10
    //verts = 20
    //halfCircles = 20

    //Sphere object was created and turned into a prefab.
    //Needed to make a empty game object and apply the script to said object (was applied to sphere object).

    public GameObject prefab;
    public int radius;
    public int verts;
    public int halfCircles;
    public List<Vertex> vertices; //acts like render geometry Vertex pointer
    public float pi = 3.14159f; //pi variable

    public struct Vertex
    {
        public Vector3 position; //vec4 variable from render geometry turned to vec3
    }

    void Start () //acts like the createSphere function
    {
        vertices = new List<Vertex>();
        List<Vertex> halfCircleVerts = generateHalfCircleVerts(verts, radius);
        vertices = generateSphereVerts(verts, halfCircles, halfCircleVerts);
        drawSphere(vertices);
    }

    List<Vertex> generateHalfCircleVerts(int np, int rad) //refactored rendering geometry function
    {
        List<Vertex> verts = new List<Vertex>();

        for (int i = 0; i < np; i++)
        {
            float angle = (pi * i) / (np - 1);
            Vertex pos = new Vertex();
            pos.position = new Vector3(rad * Mathf.Cos(angle), rad * Mathf.Sin(angle), 0);
            verts.Add(pos);
        }
        return verts;
    }

    List<Vertex> generateSphereVerts(int sides, int mirid, List<Vertex> halfCircle) //refactored rendering geometry function
    {
        int count = 0;
        List<Vertex> verts = new List<Vertex>();

        for (int i = 0; i < mirid; i++)
        {
            float phi = (2.0f * pi) * ((float)i / (float)(mirid));
            for (int j = 0; j < sides; j++, count++)
            {
                float x = halfCircle[j].position.x;
                float y = halfCircle[j].position.y * Mathf.Cos(phi) - halfCircle[j].position.z * Mathf.Sin(phi);
                float z = halfCircle[j].position.z * Mathf.Cos(phi) + halfCircle[j].position.y * Mathf.Sin(phi);

                Vertex pos = new Vertex();
                pos.position = new Vector3(x, y, z);
                verts.Add(pos);
            }
        }
        return verts;
    }

    void drawSphere(List<Vertex> vertices) //new draw function
    {
        foreach (Vertex v in vertices)
        {                                   //used for rotations
            Instantiate(prefab, v.position, Quaternion.identity);
        }
    }
}