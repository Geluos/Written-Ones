using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderScript : MonoBehaviour
{
    public void Start()
    {
        gameObject.GetComponent<Slider>().value = SoundController.main.MusicVolume;
        gameObject.GetComponent<Slider>().onValueChanged.AddListener((value) =>
        {
            SoundController.main.MusicVolume = value;
        });
    }
}
