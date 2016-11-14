using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{   //lists for monoparticles, springDampers, triangles, and drawers
    [HideInInspector]public List<MonoParticle> monoparticles;
    public List<SpringDamper> springDampers;
    public List<Triangle> triangles;
    [HideInInspector]public List<GameObject> drawers;
    
    //game objects for particles, dampers and camera
    public GameObject prefab;
    public GameObject prefabDamper;
    public GameObject prefabCamera;

    public int width; //determines how wide the grid is
    public int height; //determines how tall the grid is
    public float spacing; //adds a space between the points
    public float mass; 

    [Range(-25f, 25f)]public float gravity; 
    [Range(0f, 100f)]public float springConstant; //ks 
    [Range(0f, 10f)]public float dampingFactor; //kd 
    public float restLength; //lo //DO NOT LET USER MODIFY THIS VALUE;
    [Range(0f, 35f)]public float windStrength; //how strong the wind is
    public bool wind; //turns wind on/off
    [Range(0f, 20f)]public float tearFactor; //how strong the lines/bounds are

    //ui sliders and toggle objects
    public Slider gravitySlider;
    public Slider springConstantSlider;
    public Slider dampingFactorSlider;
    public Slider windStrengthSlider;
    public Toggle windToggle;
    public Slider tearFactorSlider;

    public void Awake()
    {   //create new lists
        monoparticles = new List<MonoParticle>();
        springDampers = new List<SpringDamper>();
        triangles = new List<Triangle>();
        drawers = new List<GameObject>();

        //create the grid
        SpawnParticles(width, height);

        //set dampers and triangles
        SetSpringDampers();
        SetTriangles();

        //lock the camera in place
        Vector3 total = Vector3.zero;
        foreach (MonoParticle mp in monoparticles)
        {
            total += mp.particle.position;
        }
        total = total / monoparticles.Count;
        total.x = 3.67f;
        total.y = 0f;
        total.z = -47.48f;
        prefabCamera.transform.position = total;
    }

    public void Start()
    {   //set the slider values
        gravitySlider.value = 25f;
        springConstantSlider.value = 100f;
        dampingFactorSlider.value = 10f;
        windStrengthSlider.value = 0f;
        tearFactorSlider.value = 10f;

        //these variables will equal the slider values
        gravity = gravitySlider.value;
        springConstant = springConstantSlider.value;
        dampingFactor = dampingFactorSlider.value;
        windStrength = windStrengthSlider.value;
        tearFactor = tearFactorSlider.value;
    }

    public void Update()
    {   //update these variables with the slider values
        gravity = gravitySlider.value;
        springConstant = springConstantSlider.value;
        dampingFactor = dampingFactorSlider.value;
        windStrength = windStrengthSlider.value;
        tearFactor = tearFactorSlider.value;
    }

    public void FixedUpdate()
    {   //create new 'temp' lists
        List<SpringDamper> tempSpringDampers = new List<SpringDamper>();
        List<Triangle> tempTriangles = new List<Triangle>();

        //add sd to 'temp' spring dampers list
        foreach (SpringDamper sd in springDampers)
        {
            tempSpringDampers.Add(sd);
        }

        //add t to 'temp' triangles list
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
            //used for tearing
            if (sd.threadTearing(tearFactor) || (sd.P1 == null || sd.P2 == null))
            {
                Destroy(drawers[springDampers.IndexOf(sd)]);
                drawers.Remove(drawers[springDampers.IndexOf(sd)]);
                springDampers.Remove(sd);
            }
        }

        foreach (Triangle t in tempTriangles)
        {
            if (windToggle.isOn) //if toggle box is checked...
            {
                wind = true;
                if (wind == true) //if wind is on...
                {
                    if (!springDampers.Contains(t.SD1) || !springDampers.Contains(t.SD2) || !springDampers.Contains(t.SD3))
                    {   
                        triangles.Remove(t);
                    }
                    else
                    {   //compute aero force
                        t.ComputeAerodynamicForce(Vector3.forward * windStrength);
                    }
                }
            }
        }

        foreach (MonoParticle mp in monoparticles)
        {  //floor boundary
            if (Camera.main.WorldToScreenPoint(mp.particle.position).y <= 10f)  
            {
                if (mp.particle.force.y < 0f)
                {
                    mp.particle.force.y = 0;
                }
                mp.particle.velocity = -mp.particle.velocity * .65f;
            }

            //ceiling boundary
            if (Camera.main.WorldToScreenPoint(mp.particle.position).y > Screen.height - 10f)  
            {
                if (mp.particle.force.y > 0f)
                {
                    mp.particle.force.y = 0;
                }  
                mp.particle.velocity = -mp.particle.velocity * .65f;
            }

            //left wall
            if (Camera.main.WorldToScreenPoint(mp.particle.position).x < 10f)    
            {
                if (mp.particle.force.x < 0f)
                {
                    mp.particle.force.x = 0;
                }
                mp.particle.velocity = -mp.particle.velocity;
            }

            //right wall
            if (Camera.main.WorldToScreenPoint(mp.particle.position).x > Screen.width - 10f)    
            {
                if (mp.particle.force.x > 0f)
                {
                    mp.particle.force.x = 0;
                } 
                mp.particle.velocity = -mp.particle.velocity * .65f;
            }

            //update particle if not an anchor. 
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

    public void LateUpdate()
    {   //create new 'temp' list
        List<GameObject> tempSpringDampers = new List<GameObject>();

        //add go to 'temp' list
        foreach (GameObject go in drawers)
        {
            tempSpringDampers.Add(go);
        }

        //setup line renderers
        for (int i = 0; i < tempSpringDampers.Count; i++)
        {
            if (tempSpringDampers[i] != null)
            {
                LineRenderer lr = tempSpringDampers[i].GetComponent<LineRenderer>();
                lr.SetPosition(0, springDampers[i].P1.position);
                lr.SetPosition(1, springDampers[i].P2.position);
            }
        }
    }

    public void SpawnParticles(int w, int h) //takes in width and height values
    {
        float x = 0f;
        float y = 0f;
        int count = 0;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++, count++)
            {   
                GameObject temp = Instantiate(prefab, new Vector3(x, y, 0), new Quaternion()) as GameObject; //instantiate game object
                MonoParticle mp = temp.GetComponent<MonoParticle>(); //get component
                mp.particle = new Particle(new Vector3(x, y, 0), new Vector3(0, 0, 0), 1); //create new particle
                monoparticles.Add(mp.GetComponent<MonoParticle>()); //add particle
                x += 1f + spacing; //add small gap
            }
            x = 0f;
            y += 1f + spacing; //add small gap
        }
        //anchor points
        monoparticles[monoparticles.Count - 1].anchorPoint = true;
        monoparticles[monoparticles.Count - w].anchorPoint = true;
        monoparticles[0].anchorPoint = true;
        monoparticles[width - 1].anchorPoint = true;
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
                drawers.Add(CreateDrawer(sd));
                springDampers.Add(sd);
            }

            if (index + width < monoparticles.Count)
            { //creates a vertical line
                mp.particle.particles.Add(monoparticles[index + width].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width].particle, springConstant, dampingFactor, restLength);
                drawers.Add(CreateDrawer(sd));
                springDampers.Add(sd);
            }

            if (index + width - 1 < monoparticles.Count && index - 1 >= 0 && (index - 1) % width < index % width)
            { //creates a diagonal line (top left)
                mp.particle.particles.Add(monoparticles[index + width - 1].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width - 1].particle, springConstant, dampingFactor, restLength);
                drawers.Add(CreateDrawer(sd));
                springDampers.Add(sd);
            }

            if (index + width + 1 < monoparticles.Count && (index + 1) % width > index % width)
            { //creates a diagonal line (top right)
                mp.particle.particles.Add(monoparticles[index + width + 1].particle);
                SpringDamper sd = new SpringDamper(mp.particle, monoparticles[index + width + 1].particle, springConstant, dampingFactor, restLength);
                drawers.Add(CreateDrawer(sd));
                springDampers.Add(sd);
            }
        }
    }

    public void SetTriangles()
    { //first check of triangles
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
        //second check of triangles
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
    //find particle index
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
    //find triangle index
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

    public GameObject CreateDrawer(SpringDamper sd)//takes in sd value
    { //used for line renderers
        GameObject drawerGO = Instantiate(prefabDamper, (sd.P1.position + sd.P2.position) / 2f, new Quaternion()) as GameObject; //instantiate game object
        LineRenderer lr = drawerGO.GetComponent<LineRenderer>();
        lr.materials[0].color = Color.black; //the color of the line renderers. //will be black
        lr.SetWidth(.1f, .1f);
        return drawerGO;
    }
}