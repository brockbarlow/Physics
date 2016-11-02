using UnityEngine;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public GameObject prefab;
    public float springConstant; //ks //set between 0 to 100;
    public float dampingFactor; //kd //set between 0 to 10;
    public float restLength; //lo //set between 0 to 25;
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
        SetSpringDampers();
    }

    public void FixedUpdate()
    {
        foreach (MonoParticle mp in particles)
        { //for each particle: apply gravity
            mp.particle.force = Vector3.zero;
            mp.particle.force = (gravity * Vector3.down) * mp.particle.mass;
            if (mp.anchor == false)
            {
                mp.transform.position = mp.particle.Update();
            }
            else
            {
                mp.transform.position = mp.transform.position;
            }
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
                mp.particle = new Particle(new Vector3(x, y, 0), new Vector3(0, 0, 0), 1);
                particles.Add(temp.GetComponent<MonoParticle>());
                x += 1f + spacing;
            }
            x = 0f;
            y += 1f + spacing;
        }
        particles[particles.Count - 1].anchor = true;
        particles[particles.Count - w].anchor = true;
    }

    public void SetSpringDampers()
    {
        foreach (MonoParticle mp in particles)
        {
            int index = FindIndex(particles, mp);
            mp.neighbors = new List<MonoParticle>();
            if ((index + 1) % width > index % width)
            { //creates a horizontal line
                mp.neighbors.Add(particles[index + 1]);
                SpringDamper sd = new SpringDamper(mp.particle, particles[index + 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width < particles.Count)
            { //created a vertical line
                mp.neighbors.Add(particles[index + width]);
                SpringDamper sd = new SpringDamper(mp.particle, particles[index + width].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width - 1 < particles.Count && index - 1 >= 0 && (index - 1) % width < index % width)
            {
                mp.neighbors.Add(particles[index + width - 1]);
                SpringDamper sd = new SpringDamper(mp.particle, particles[index + width - 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width + 1 < particles.Count && (index + 1) % width > index % width)
            {
                mp.neighbors.Add(particles[index + width + 1]);
                SpringDamper sd = new SpringDamper(mp.particle, particles[index + width + 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
        }
    }

    public int FindIndex(List<MonoParticle> list, MonoParticle mp)
    {
        int index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == mp)
            {
                index = i;
                break;
            }
        }
        return index;
    }
}