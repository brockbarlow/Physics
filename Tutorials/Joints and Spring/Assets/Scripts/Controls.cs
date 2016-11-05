using UnityEngine;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public GameObject prefab;
    public float springConstant; //ks //set between 0 to 100;
    public float dampingFactor; //kd //set between 0 to 10;
    public float restLength; //lo //set between 0 to 25;
    public float windStrength;
    public bool wind;
    public int windCount;
    public float mass;
    public float gravity;
    public float spacing;
    public int width;
    public int height;
    public List<MonoParticle> monoparticles;
    public List<SpringDamper> springDampers;
    public List<Triangle> triangles;

    public void Awake()
    {
        monoparticles = new List<MonoParticle>();
        springDampers = new List<SpringDamper>();
        triangles = new List<Triangle>();
        SpawnParticles(width, height);
        SetSpringDampers();
        SetTriangles();
    }

    public void FixedUpdate()
    {
        foreach (MonoParticle mp in monoparticles)
        { //for each particle: apply gravity
            mp.particle.force = Vector3.zero;
            mp.particle.force = (gravity * Vector3.down) * mp.particle.mass;
            if (mp.anchorPoints == false)
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
        foreach (Triangle t in triangles)
        {
            if (wind)
            {
                if (!springDampers.Contains(t.SD1) || !springDampers.Contains(t.SD2) || !springDampers.Contains(t.SD3))
                {
                    triangles.Remove(t);
                }
                else
                {
                    t.CalculateAerodynamicForce(Vector3.forward * windStrength);
                }
            }
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
                monoparticles.Add(temp.GetComponent<MonoParticle>());
                x += 1f + spacing;
            }
            x = 0f;
            y += 1f + spacing;
        }
        monoparticles[monoparticles.Count - 1].anchorPoints = true;
        monoparticles[monoparticles.Count - w].anchorPoints = true;
        monoparticles[0].anchorPoints = true;
        monoparticles[width - 1].anchorPoints = true;
    }

    public void SetSpringDampers()
    {
        foreach (MonoParticle mp in monoparticles)
        {
            int index = FindIndex(monoparticles, mp);
            mp.particle.particles = new List<Particle>();
            if ((index + 1) % width > index % width)
            { //creates a horizontal line
                mp.particle.particles.Add(monoparticles[index + 1].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width < monoparticles.Count)
            { //created a vertical line
                mp.particle.particles.Add(monoparticles[index + width].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width - 1 < monoparticles.Count && index - 1 >= 0 && (index - 1) % width < index % width)
            {
                mp.particle.particles.Add(monoparticles[index + width - 1].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width - 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
            if (index + width + 1 < monoparticles.Count && (index + 1) % width > index % width)
            {
                mp.particle.particles.Add(monoparticles[index + width + 1].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width + 1].particle, springConstant, dampingFactor, restLength);
                springDampers.Add(sd);
            }
        }
    }

    public void SetTriangles()
    {
        foreach (MonoParticle mp in monoparticles)
        {
            int index = FindIndex(monoparticles, mp);
            Triangle t;
            if (index % width != width - 1 && index + width < monoparticles.Count)
            {
                t = new Triangle(monoparticles[index], monoparticles[index + 1], monoparticles[index + width]);
                foreach (SpringDamper sd in springDampers)
                {
                    if ((sd.P1 == t.TP1 && sd.P2 == t.TP2) || (sd.P1 == t.TP2 && sd.P2 == t.TP1))
                    {
                        t.SD1 = sd;
                    }
                    else if ((sd.P1 == t.TP2 && sd.P2 == t.TP3) || (sd.P1 == t.TP3 && sd.P2 == t.TP2))
                    {
                        t.SD2 = sd;
                    }
                    else if ((sd.P1 == t.TP3 && sd.P2 == t.TP1) || (sd.P1 == t.TP1 && sd.P2 == t.TP3))
                    {
                        t.SD3 = sd;
                    }
                }
                triangles.Add(t);
            }
        }
        foreach (MonoParticle mp in monoparticles)
        {
            int index = FindIndex(monoparticles, mp);
            Triangle t;
            if (index >= width && index + 1 < monoparticles.Count && index % width != width - 1)
            {
                t = new Triangle(monoparticles[index], monoparticles[index + 1], monoparticles[index - width + 1]);
                triangles.Add(t);
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

    public int FindIndex(List<Triangle> list, Triangle t)
    {
        int index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == t)
            {
                index = i;
                break;
            }
        }
        return index;
    }
}