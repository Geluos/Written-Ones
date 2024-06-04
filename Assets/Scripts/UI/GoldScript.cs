using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldScript : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    public void UpdateValue(int newValue)
    {
        goldText.text = newValue.ToString();
    }
}
