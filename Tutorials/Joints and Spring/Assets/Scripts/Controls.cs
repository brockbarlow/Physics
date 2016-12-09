namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using System.Linq;

    public class Controls : MonoBehaviour
    {   //lists for monoparticles, springDampers, triangles, and drawers
        [HideInInspector]public List<MonoParticle> Monoparticles;
        public List<SpringDamper> SpringDampers;
        public List<Triangle> Triangles;
        [HideInInspector]public List<GameObject> Drawers;

        //game objects for particles, dampers and camera
        public GameObject Prefab;
        public GameObject PrefabDamper;
        public GameObject PrefabCamera;

        public int Width; //determines how wide the grid is
        public int Height; //determines how tall the grid is
        public float Spacing; //adds a space between the points
        public float Mass;

        public float Gravity;
        [Range(0f, 100f)]public float SpringConstant; //ks 
        [Range(0f, 10f)]public float DampingFactor; //kd 
        public float RestLength; //lo //DO NOT LET USER MODIFY THIS VALUE;
        [Range(0f, 15f)]public float WindStrength; //how strong the wind is
        public bool Wind; //turns wind on/off
        [Range(0.01f, 10f)]public float TearFactor; //how strong the lines/bounds are

        //ui sliders and toggle objects
        public Slider SpringConstantSlider;
        public Slider DampingFactorSlider;
        public Slider WindStrengthSlider;
        public Toggle WindToggle;
        public Slider TearFactorSlider;

        public void Awake()
        {   //create new lists
            Monoparticles = new List<MonoParticle>();
            SpringDampers = new List<SpringDamper>();
            Triangles = new List<Triangle>();
            Drawers = new List<GameObject>();

            //create the grid
            SpawnParticles(Width, Height);

            //set dampers and triangles
            SetSpringDampers();
            SetTriangles();

            //lock the camera in place
            var total = Monoparticles.Aggregate(Vector3.zero, (current, mp) => current + mp.Particle.Position);
            total = total / Monoparticles.Count;
            total.x = 3.67f;
            total.y = 0f;
            total.z = -47.48f;
            PrefabCamera.transform.position = total;
        }

        public void Start()
        {   //set the slider values
            SpringConstantSlider.value = 100f;
            DampingFactorSlider.value = 10f;
            WindStrengthSlider.value = 0f;
            TearFactorSlider.value = 10f;

            //these variables will equal the slider values
            SpringConstant = SpringConstantSlider.value;
            DampingFactor = DampingFactorSlider.value;
            WindStrength = WindStrengthSlider.value;
            TearFactor = TearFactorSlider.value;
        }

        public void Update()
        {   //update these variables with the slider values
            SpringConstant = SpringConstantSlider.value;
            DampingFactor = DampingFactorSlider.value;
            WindStrength = WindStrengthSlider.value;
            TearFactor = TearFactorSlider.value;
        }

        public void FixedUpdate()
        {   //create new 'temp' lists
            var tempSpringDampers = SpringDampers.ToList();

            //add sd to 'temp' spring dampers list

            //add t to 'temp' triangles list
            var tempTriangles = Triangles.ToList();

            foreach (var mp in Monoparticles)
            { //for each particle: apply gravity
                mp.Particle.Force = Vector3.zero;
                mp.Particle.Force = (Gravity * Vector3.down) * mp.Particle.Mass;
            }

            foreach (var sd in tempSpringDampers)
            { //for each spring-damper: compute & apply forces
                sd.SpringConstant = SpringConstant;
                sd.DampingFactor = DampingFactor;
                sd.RestLength = RestLength;
                sd.ComputeForce();
                //used for tearing
                if (!sd.ThreadTearing(TearFactor) && (sd.P1 != null && sd.P2 != null)) continue;
                Destroy(Drawers[SpringDampers.IndexOf(sd)]);
                Drawers.Remove(Drawers[SpringDampers.IndexOf(sd)]);
                SpringDampers.Remove(sd);
            }

            foreach (var t in tempTriangles)
            {
                if (!WindToggle.isOn) continue;
                Wind = true;
                if (!Wind) continue;
                if (!SpringDampers.Contains(t.Sd1) || !SpringDampers.Contains(t.Sd2) || !SpringDampers.Contains(t.Sd3))
                {
                    Triangles.Remove(t);
                }
                else
                {   //compute aero force
                    t.ComputeAerodynamicForce(Vector3.forward * WindStrength);
                }
            }

            foreach (var mp in Monoparticles)
            {  //floor boundary
                if (Camera.main.WorldToScreenPoint(mp.Particle.Position).y <= 10f)
                {
                    if (mp.Particle.Force.y < 0f)
                    {
                        mp.Particle.Force.y = 0;
                    }
                    mp.Particle.Velocity = -mp.Particle.Velocity * .65f;
                }

                //ceiling boundary
                if (Camera.main.WorldToScreenPoint(mp.Particle.Position).y > Screen.height - 10f)
                {
                    if (mp.Particle.Force.y > 0f)
                    {
                        mp.Particle.Force.y = 0;
                    }
                    mp.Particle.Velocity = -mp.Particle.Velocity * .65f;
                }

                //left wall
                if (Camera.main.WorldToScreenPoint(mp.Particle.Position).x < 10f)
                {
                    if (mp.Particle.Force.x < 0f)
                    {
                        mp.Particle.Force.x = 0;
                    }
                    mp.Particle.Velocity = -mp.Particle.Velocity;
                }

                //right wall
                if (Camera.main.WorldToScreenPoint(mp.Particle.Position).x > Screen.width - 10f)
                {
                    if (mp.Particle.Force.x > 0f)
                    {
                        mp.Particle.Force.x = 0;
                    }
                    mp.Particle.Velocity = -mp.Particle.Velocity * .65f;
                }

                //update particle if not an anchor. 
                if (mp.AnchorPoint == false)
                {
                    mp.transform.position = mp.Particle.UpdateParticle();
                }
                else
                {
                    mp.Particle.Position = mp.transform.position;
                }
            }
        }

        public void LateUpdate()
        {   //create new 'temp' list
            var tempSpringDampers = Drawers.ToList();

            //add go to 'temp' list

            //setup line renderers
            for (var i = 0; i < tempSpringDampers.Count; i++)
            {
                if (tempSpringDampers[i] == null) continue;
                var lr = tempSpringDampers[i].GetComponent<LineRenderer>();
                lr.SetPosition(0, SpringDampers[i].P1.Position);
                lr.SetPosition(1, SpringDampers[i].P2.Position);
            }
        }

        public void SpawnParticles(int w, int h) //takes in width and height values
        {
            var x = 0f;
            var y = 0f;

            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++/*, count++*/)
                {
                    var temp = Instantiate(Prefab, new Vector3(x, y, 0), new Quaternion()) as GameObject; //instantiate game object
                    if (temp != null)
                    {
                        var mp = temp.GetComponent<MonoParticle>(); //get component
                        mp.Particle = new Particle(new Vector3(x, y, 0), new Vector3(0, 0, 0), Mass); //create new particle
                        Monoparticles.Add(mp.GetComponent<MonoParticle>()); //add particle
                    }
                    x += 1f + Spacing; //add small gap
                }
                x = 0f;
                y += 1f + Spacing; //add small gap
            }
            //anchor points
            Monoparticles[Monoparticles.Count - w].AnchorPoint = true;
            Monoparticles[Monoparticles.Count - 1].AnchorPoint = true;
            Monoparticles[Monoparticles.Count - 2].AnchorPoint = true;
            Monoparticles[Monoparticles.Count - 3].AnchorPoint = true;
            Monoparticles[Monoparticles.Count - 4].AnchorPoint = true;
        }

        public void SetSpringDampers()
        {
            foreach (var mp in Monoparticles)
            {
                var index = FindIndex(Monoparticles, mp);
                mp.Particle.Particles = new List<Particle>();

                if ((index + 1) % Width > index % Width)
                { //creates a horizontal line
                    mp.Particle.Particles.Add(Monoparticles[index + 1].Particle);
                    var sd = new SpringDamper(mp.Particle, Monoparticles[index + 1].Particle, SpringConstant, DampingFactor, RestLength);
                    Drawers.Add(CreateDrawer(sd));
                    SpringDampers.Add(sd);
                }

                if (index + Width < Monoparticles.Count)
                { //creates a vertical line
                    mp.Particle.Particles.Add(Monoparticles[index + Width].Particle);
                    var sd = new SpringDamper(mp.Particle, Monoparticles[index + Width].Particle, SpringConstant, DampingFactor, RestLength);
                    Drawers.Add(CreateDrawer(sd));
                    SpringDampers.Add(sd);
                }

                if (index + Width - 1 < Monoparticles.Count && index - 1 >= 0 && (index - 1) % Width < index % Width)
                { //creates a diagonal line (top left)
                    mp.Particle.Particles.Add(Monoparticles[index + Width - 1].Particle);
                    var sd = new SpringDamper(mp.Particle, Monoparticles[index + Width - 1].Particle, SpringConstant, DampingFactor, RestLength);
                    Drawers.Add(CreateDrawer(sd));
                    SpringDampers.Add(sd);
                }

                if (index + Width + 1 >= Monoparticles.Count || (index + 1)%Width <= index%Width) continue;
                {
//creates a diagonal line (top right)
                    mp.Particle.Particles.Add(Monoparticles[index + Width + 1].Particle);
                    var sd = new SpringDamper(mp.Particle, Monoparticles[index + Width + 1].Particle, SpringConstant, DampingFactor, RestLength);
                    Drawers.Add(CreateDrawer(sd));
                    SpringDampers.Add(sd);
                }
            }
        }

        public void SetTriangles()
        { //first check of triangles
            foreach (var mp in Monoparticles)
            {
                var index = FindIndex(Monoparticles, mp);

                if (index%Width == Width - 1 || index + Width >= Monoparticles.Count) continue;
                var t = new Triangle(Monoparticles[index], Monoparticles[index + 1], Monoparticles[index + Width]);

                foreach (var sd in SpringDampers)
                {
                    if ((sd.P1 == t.Tp1 && sd.P2 == t.Tp2) || (sd.P1 == t.Tp2 && sd.P2 == t.Tp1))
                    {
                        t.Sd1 = sd;
                    }
                    else if ((sd.P1 == t.Tp2 && sd.P2 == t.Tp3) || (sd.P1 == t.Tp3 && sd.P2 == t.Tp2))
                    {
                        t.Sd2 = sd;
                    }
                    else if ((sd.P1 == t.Tp3 && sd.P2 == t.Tp1) || (sd.P1 == t.Tp1 && sd.P2 == t.Tp3))
                    {
                        t.Sd3 = sd;
                    }
                }
                Triangles.Add(t);
            }
            //second check of triangles
            foreach (var mp in Monoparticles)
            {
                var index = FindIndex(Monoparticles, mp);

                if (index < Width || index + 1 >= Monoparticles.Count || index%Width == Width - 1) continue;
                var t = new Triangle(Monoparticles[index], Monoparticles[index + 1], Monoparticles[index - Width + 1]);
                Triangles.Add(t);
            }
        }
        //find particle index
        public int FindIndex(List<MonoParticle> list, MonoParticle mp)
        {
            var index = 0;

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != mp) continue;
                index = i;
                break;
            }
            return index;
        }
        //find triangle index
        public int FindIndex(List<Triangle> list, Triangle t)
        {
            var index = 0;

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != t) continue;
                index = i;
                break;
            }
            return index;
        }

        public GameObject CreateDrawer(SpringDamper sd)//takes in sd value
        { //used for line renderers
            var drawerGo = Instantiate(PrefabDamper, (sd.P1.Position + sd.P2.Position) / 2f, new Quaternion()) as GameObject; //instantiate game object
            if (drawerGo == null) return null;
            var lr = drawerGo.GetComponent<LineRenderer>();
            lr.materials[0].color = Color.black; //the color of the line renderers. //will be black
            lr.SetWidth(.1f, .1f);
            return drawerGo;
        }
    }
}