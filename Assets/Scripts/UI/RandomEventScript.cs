using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RandomEventScript : MonoBehaviour
{
    public GameObject image;
    public GameObject text;
    public GameObject switchLeft;
    public GameObject switchRight;
    public GameObject sumbit;

    private List<EffectObject> effects;
    private int currentIndex = 0;

    public void Activate(EffectsCombination effectsCombination)
    {
        effects = effectsCombination.effects;
        switchLeft.GetComponent<Button>().onClick.AddListener(() => {
            if (currentIndex > 0)
            {
                --currentIndex;
                Load(currentIndex);
            }

            if (currentIndex == 0)
                switchLeft.GetComponent<Button>().interactable = false;

            if (currentIndex == (effects.Count - 2))
                switchRight.GetComponent<Button>().interactable = true;
        });

        switchRight.GetComponent<Button>().onClick.AddListener(() => {
            if (currentIndex < (effects.Count - 1))
            {
                ++currentIndex;
                Load(currentIndex);
            }

            if (currentIndex == (effects.Count - 1))
                switchRight.GetComponent<Button>().interactable = false;

            if (currentIndex == 1)
                switchLeft.GetComponent<Button>().interactable = true;
        });

        sumbit.GetComponent<Button>().onClick.AddListener(() => {
            effects[currentIndex].effect.Activate();
            Destroy(gameObject);
        });

        Load(currentIndex);
        switchLeft.GetComponent<Button>().interactable = false;
        if (effects.Count == 1)
            switchRight.GetComponent<Button>().interactable = false;
    }

    private void Load(int effectIdx)
    {
        image.GetComponent<Image>().sprite = effects[effectIdx].image;
        text.GetComponent<TextMeshProUGUI>().text = effects[effectIdx].text;
    }
}
