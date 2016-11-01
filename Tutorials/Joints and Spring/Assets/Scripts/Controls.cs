using UnityEngine;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public GameObject prefab;
    public float springConstant; //ks
    public float dampingFactor; //kd
    public float restLength; //lo
    public float gravity;
    public float spacing;
    public int width;
    public int height;
    private List<MonoParticle> particles;
    private List<SpringDamper> springDampers;

    public void Awake()
    {
        particles = new List<MonoParticle>();
        springDampers = new List<SpringDamper>();

        SpawnParticles(width, height);
    }

    public void FixedUpdate()
    {
        foreach (MonoParticle mp in particles)
        { //for each particle: apply gravity
            mp.particle.UpdateParticle(gravity);
        }
        foreach (SpringDamper sd in springDampers)
        { //for each spring-damper: compute & apply forces
            sd.ComputeForce();
            sd.springConstant = springConstant;
            sd.dampingFactor = dampingFactor;
            sd.restLength = restLength;
        }
    }

    public void SpawnParticles(int w, int h)
    {
        float x = 0f;
        float y = 0f;
        int count = 0;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++, count++)
            {
                GameObject temp = Instantiate(prefab, new Vector3(x, y, 0), new Quaternion()) as GameObject;
                MonoParticle mp = temp.GetComponent<MonoParticle>();
                mp.particle = new Particle(new Vector3(x, y, 0), new Vector3(0, 0, 0), 10);
                particles.Add(temp.GetComponent<MonoParticle>());
                x += 1f + spacing;
            }
            x = 0f;
            y += 1f + spacing;
        }
    }
}