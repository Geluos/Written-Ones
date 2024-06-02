using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
	public Character character;
	public TMPro.TextMeshProUGUI hpValueText;
	public Image shieldImage;
	public TMPro.TextMeshProUGUI shieldText;

	void Start()
	{
		SetMaxHealth();
	}
	
    public void SetMaxHealth()
    {
		if (character!=null)
		{
			slider.maxValue = character.max_hp;
			hpValueText.text = character.max_hp.ToString();
			slider.value = character.current_hp;
		}
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

	public void SetShield()
	{
		if (character != null)
		{
			if (character.shield > 0)
			{
				shieldText.enabled = true;
				shieldImage.enabled = true;

				shieldText.text = character.shield.ToString();
			}
			else
			{

				shieldText.enabled = false;
				shieldImage.enabled = false;
			}
		}
	}

	void Update()
	{
		SetHealth();
		SetShield();
	}
}
