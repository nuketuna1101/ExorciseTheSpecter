using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandArea : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerUpHandler, IPointerDownHandler,
    IEndDragHandler, IDropHandler
{

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("OnPointerEnter on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Debug.Log("OnPointerExit on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        DebugOpt.Log("OnPointerUp on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        DebugOpt.Log("OnPointerDown on " + this.name + " : " + pointerEventData.position);
    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        DebugOpt.Log("OnEndDrag on " + this.name + " : " + pointerEventData.position);
    }
    public void OnDrop(PointerEventData pointerEventData)
    {
        DebugOpt.Log("OnDrop on " + this.name + " : " + pointerEventData.position);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        DebugOpt.Log("OnCollisionExit2D on " + this.name + " : " + collision.gameObject.name);

    }

}
