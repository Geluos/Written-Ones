using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCardScript : CardBaseScript
{
    public new void OnBeginDrag(PointerEventData eventData) { }

    public new void OnDrag(PointerEventData eventData) { }

    public new void OnEndDrag(PointerEventData eventData) { }

    public void OnMouseUp()
    {
        Debug.Log("SHOP");
    }
}
