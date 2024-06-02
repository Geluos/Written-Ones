using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBuyer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private AdventureController adventureController;

    void Start()
    {
        adventureController = FindObjectOfType<AdventureController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��������� ������ ��������������
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��������� ��������������
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ��������� ��������� ��������������
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HandleCardClick();
    }

    private void HandleCardClick()
    {
        CardBaseScript card = GetComponent<CardBaseScript>();
        if (card != null && adventureController != null)
        {
            // TODO ��������
        }
        Debug.Log($"{gameObject.name} was clicked!");

    }
}
