using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SliderControls : MonoBehaviour
{
    [HideInInspector]public List<Controls> controls;
    public Slider springConstant;
    public Slider dampingFactor;
    public Slider restLength;
    public Slider windStrength;

    public void Start()
    {
        foreach (Controls c in controls)
        {
            c.springConstant = 100f;
            c.dampingFactor = 10;
            c.restLength = 2;
            c.windStrength = 0f;
        }
    }

    public void Update()
    {
        foreach (Controls c in controls)
        {
            c.springConstant = springConstant.value;
            c.dampingFactor = dampingFactor.value;
            c.restLength = restLength.value;
            c.windStrength = windStrength.value;
        }
    }
}