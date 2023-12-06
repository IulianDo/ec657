using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    //references
    [SerializeField] Image fill;
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    // Start is called before the first frame update

    //increase slider max value and set value to max
    public void setMaxHealth(float maxHealth)
	{
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        fill.color = gradient.Evaluate(1f);
	}

    //set slider values to what hp is
    public void setHealth(int health)
	{
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}
