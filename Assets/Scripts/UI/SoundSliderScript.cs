using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderScript : MonoBehaviour
{
    public void Start()
    {
        gameObject.GetComponent<Slider>().value = SoundController.main.EffectsVolume;
        gameObject.GetComponent<Slider>().onValueChanged.AddListener((value) =>
        {
            SoundController.main.EffectsVolume = value;
        });
    }
}
