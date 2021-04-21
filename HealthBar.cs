using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFillColor;

    public void SetMaxHealth(int healthValue)
    {
        healthSlider.maxValue = healthValue;
        healthSlider.value = healthValue;

        healthFillColor.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int healthValue)
    {
        healthSlider.value = healthValue;

        healthFillColor.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
