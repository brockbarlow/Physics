using UnityEngine;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    [HideInInspector]public List<MonoParticle> monoparticles;
    public List<SpringDamper> springDampers;
    public List<Triangle> triangles;
    public GameObject prefab;
    public int width;
    public int height;
    public float spacing; //adds a space between the points
    public float mass;
    public float gravity;
    [Range(0f, 100f)]public float springConstant; //ks //set between 0 to 100;
    [Range(0f, 10f)]public float dampingFactor; //kd //set between 0 to 10;
    [Range(0f, 25f)]public float restLength; //lo //set between 0 to 25;
    [Range(0.01f, 10f)]public float windStrength;
    public bool wind;

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
        List<SpringDamper> tempSpringDampers = new List<SpringDamper>();
        List<Triangle> tempTriangles = new List<Triangle>();
        foreach (SpringDamper sd in springDampers)
        {
            tempSpringDampers.Add(sd);
        }
        foreach (Triangle t in triangles)
        {
            tempTriangles.Add(t);
        }
        foreach (MonoParticle mp in monoparticles)
        { //for each particle: apply gravity
            mp.particle.force = Vector3.zero;
            mp.particle.force = (gravity * Vector3.down) * mp.particle.mass;
        }
        foreach (SpringDamper sd in tempSpringDampers)
        { //for each spring-damper: compute & apply forces
            sd.springConstant = springConstant;
            sd.dampingFactor = dampingFactor;
            sd.restLength = restLength;
            sd.ComputeForce();
        }
        foreach (Triangle t in tempTriangles)
        {
            if (wind)
            {
                if (!springDampers.Contains(t.SD1) || !springDampers.Contains(t.SD2) || !springDampers.Contains(t.SD3))
                {
                    triangles.Remove(t);
                }
                else
                {
                    t.ComputeAerodynamicForce(Vector3.forward * windStrength);
                }
            }
        }
        foreach (MonoParticle mp in monoparticles)
        {
            if (mp.anchorPoint == false)
            {
                mp.transform.position = mp.particle.UpdateParticle();
            }
            else
            {
                mp.particle.position = mp.transform.position;
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
                monoparticles.Add(mp.GetComponent<MonoParticle>());
                x += 1f + spacing;
            }
            x = 0f;
            y += 1f + spacing;
        }
        monoparticles[monoparticles.Count - 1].anchorPoint = true;
        monoparticles[monoparticles.Count - w].anchorPoint = true;
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