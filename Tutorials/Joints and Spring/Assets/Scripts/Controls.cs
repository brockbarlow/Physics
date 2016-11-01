using UnityEngine;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public GameObject prefab;
    public float springConstant; //ks
    public float dampingFactor; //kd
    public float restLength; //lo
    public float gravity;
    public float padding;
    public int width;
    public int height;
    private List<MonoParticle> particles;
    private List<SpringDamper> springDampers;

    public void Start()
    {
        particles = new List<MonoParticle>();
        springDampers = new List<SpringDamper>();

        SpawnParticles(width, height);
    }

    public void Update()
    {

    }

    public void SpawnParticles(int w, int h)
    {

    }
}