using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardButtonScript : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI goldButtonText;
    public Action onSelectionEnd;
    private int goldAmount;
    public int GoldAmount
    {
        set
        {
            goldAmount = value;
            goldButtonText.text = $"{value} золота";
        }
        get => goldAmount;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GoldController.main.Add(goldAmount);
        onSelectionEnd();
    }
}
