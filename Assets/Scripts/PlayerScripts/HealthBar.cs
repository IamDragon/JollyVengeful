using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text hpText;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        SetText();
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        SetText();
    }
    public void SetText()
    {
        hpText.text = slider.value + "/" + slider.maxValue;
    }
}
