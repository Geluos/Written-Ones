using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBaseScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 offset;
    Rigidbody2D rb;

    public virtual void CardActivate() { }
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        /*var new_offset = offset;
        new_offset.x = eventData.position.x;
        new_offset.y = Camera.main.pixelHeight - eventData.position.y;
        new_offset = Camera.main.ScreenToWorldPoint(new Vector3(new_offset.x, new_offset.y, Camera.main.nearClipPlane));*/
        offset = rb.position - eventData.position;
        //offset = Camera.main.ScreenToWorldPoint(offset);
        Debug.Log($"OnBeginDrag {rb.position}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = eventData.position;
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        rb.position = newPos + offset;

        Debug.Log($"OnDrag {transform.position} {eventData.position} {offset}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }
}
