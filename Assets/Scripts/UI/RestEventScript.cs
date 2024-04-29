using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestEventScript : MonoBehaviour
{
    public GameObject text;
    public GameObject submit;

    public void Activate(int healthAddition)
    {
        text.GetComponent<TextMeshProUGUI>().text = $"������ �������� �������� ������������� {healthAddition} ��������� ��������";
        submit.GetComponent<Button>().onClick.AddListener(() => { Destroy(gameObject); });
    }
}
