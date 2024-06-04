using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarStat : MonoBehaviour
{
    public Slider slider;
    public Image fill;
	public Character character;
	public TMPro.TextMeshProUGUI hpValueText;

    private void OnEnable()
    {
        SetHealth();
    }

    private void Update()
    {
        SetHealth();
    }

    public void SetHealth()
    {
		if (character!=null)
		{
			if (slider.value != character.current_hp)
			{
				slider.value = character.current_hp;
			}
			hpValueText.text = character.current_hp.ToString();
		} 
    }
}
