namespace Assets.Scripts
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;

    public class SliderControls : MonoBehaviour
    {
        public List<BoidRules> BoidRules;
        public Slider CohesionSlider;
        public Slider DispensionSlider;
        public Slider AlignmentSlider;
        public Slider TendencySlider;

        public void Start()
        {
            CohesionSlider.value = 0f;
            DispensionSlider.value = 0f;
            AlignmentSlider.value = 0f;
            TendencySlider.value = 0f;

            foreach (var br in BoidRules)
            {
                br.Cohesion = CohesionSlider.value;
                br.Dispersion = DispensionSlider.value;
                br.Alignment = AlignmentSlider.value;
                br.Tendency = TendencySlider.value;
            }
        }

        public void Update()
        {
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