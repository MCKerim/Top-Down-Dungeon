using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxValue(float number)
    {
        slider.maxValue = number;
        slider.value = number;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float number)
    {
        slider.value = number;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
