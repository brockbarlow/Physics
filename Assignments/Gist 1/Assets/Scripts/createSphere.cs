/*gist1: refactor your c++ generate halfcircle and generate sphere functions to c# then execute them in unityengine*/

using UnityEngine;
using System.Collections.Generic;

public class createSphere : MonoBehaviour {

    public GameObject prefab;
    public int radius;
    public int verts;
    public int halfCircles;
    public List<Vertex> vertices;
    public float pi = 3.14159f;

    public struct Vertex
    {
        public Vector3 position;
    }

    void Start ()
    {
        vertices = new List<Vertex>();
        List<Vertex> halfCircleVerts = generateHalfCircleVerts(verts, radius);
        vertices = generateSphereVerts(verts, halfCircles, halfCircleVerts);
    }

    List<Vertex> generateHalfCircleVerts(int np, int rad)
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

    List<Vertex> generateSphereVerts(int sides, int mirid, List<Vertex> halfCircle)
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
}