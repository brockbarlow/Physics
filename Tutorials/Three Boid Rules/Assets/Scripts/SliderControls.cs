using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SliderControls : MonoBehaviour
{
    public List<BoidRules> boidRules;
    public Slider cohesion;
    public Slider dispension;
    public Slider alignment;
    public Slider tendency;

    public void Start()
    {
        foreach (BoidRules br in boidRules)
        {
            br.cohesion = cohesion.value;
            br.dispersion = dispension.value;
            br.alignment = alignment.value;
            br.tendency = 0f;
        }
    }

    public void Update()
    {
        foreach (BoidRules br in boidRules)
        {
            br.cohesion = cohesion.value;
            br.dispersion = dispension.value;
            br.alignment = alignment.value;
            br.tendency = tendency.value;
        }
    }
}