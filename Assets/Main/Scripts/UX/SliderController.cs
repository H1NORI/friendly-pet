using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Text valueText;
    int progress = 0;
    public Slider slider;

    public void OnSiliderChanged(float value)
    {
        valueText.text = value.ToString();
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
    }

    public void RemoveProgress()
    {
        progress--;
        slider.value = progress;
    }
}
