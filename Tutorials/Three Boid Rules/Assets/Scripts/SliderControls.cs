namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;

    public class SliderControls : MonoBehaviour
    {   //boidrules list
        public List<BoidRules> BoidRules;
        //ui slider elements
        public Slider CohesionSlider;
        public Slider DispensionSlider;
        public Slider AlignmentSlider;
        public Slider TendencySlider;

        public void Start()
        {   //set the slider values to zero on runtime
            CohesionSlider.value = 0f;
            DispensionSlider.value = 0f;
            AlignmentSlider.value = 0f;
            TendencySlider.value = 0f;
        }

        public void Update()
        {   //when a slider value changes, boids will be effected/updated
            foreach (var br in BoidRules)
            {
                br.Cohesion = CohesionSlider.value;
                br.Dispersion = DispensionSlider.value;
                br.Alignment = AlignmentSlider.value;
                br.Tendency = TendencySlider.value;
            }
        }
    }
}