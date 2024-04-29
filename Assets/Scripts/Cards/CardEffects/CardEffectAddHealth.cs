using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectAddHealth", menuName = "CardEffect/CardEffectAddHealth", order = -50)]
public class CardEffectAddHealth : CardEffect
{
	public override void Activate()
	{
		var dialog = Instantiate(AdventureController.main.dialogCanvas);
		dialog.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 
			"Каждый персонаж получает дополнительно 40 процентов здоровья";
		dialog.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => { Destroy(dialog); });
    }
}
