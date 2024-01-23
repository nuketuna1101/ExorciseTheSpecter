using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObj : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler,
    IDragHandler, IEndDragHandler, IDropHandler,
    ISelectHandler

{
    // Start is called before the first frame update
    void Start()
    {
        //Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //transform.position = mousePosition;

        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //touchPos.z = -5f;
        //selectCard.LocateCard(touchPos, Quaternion.identity, selectCard.originalPRS.scale);

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Debug.Log("OnPointerEnter on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Debug.Log("OnPointerExit on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Debug.Log("OnPointerClick on " + this.name + " : " + pointerEventData.position);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log("OnPointerUp on " + this.name + " : " + pointerEventData.position);
        //Debug.Log("OnPointerUp on 2 " + this.name + " : " + Camera.main.ScreenToWorldPoint(pointerEventData.position));
        //Debug.Log("OnPointerUp on 2 " + this.name + " : " + this.transform.position);

        var pointPos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        RaycastHit2D hit = Physics2D.Raycast(pointPos, transform.forward * 10);
        DebugOpt.DrawRay(pointPos, transform.forward * 10, Color.blue, 0.3f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("StaticUI"))
            {
                Debug.Log("RayCast Hit! on " + hit.collider.gameObject.name + " and z: " + hit.collider.transform.position.z);
            }
        }


        var tmpPos = pointerEventData.position;
        RaycastHit2D hit2 = Physics2D.Raycast(tmpPos, transform.forward * 10);
        DebugOpt.DrawRay(pointPos, transform.forward * 10, Color.yellow, 0.3f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("StaticUI"))
            {
                Debug.Log("RayCast Hit22! on " + hit.collider.gameObject.name + " and z: " + hit.collider.transform.position.z);
            }
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("OnPointerDown on " + this.name + " : " + pointerEventData.position);



    }
    public void OnPointerMove(PointerEventData pointerEventData)
    {
        //Debug.Log("OnPointerMove on " + this.name + " : " + pointerEventData.position);
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        //Debug.Log("OnDrag on " + this.name + " : " + pointerEventData.position);
        var tmpPos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        tmpPos.z = 0.0f;
        this.transform.position = tmpPos;

        DebugOpt.DrawRay(tmpPos, transform.forward * 15, Color.red, 0.3f);

    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        Debug.Log("OnEndDrag on " + this.name + " : " + pointerEventData.position);
    }
    public void OnDrop(PointerEventData pointerEventData)
    {
        Debug.Log("OnDrop on " + this.name + " : " + pointerEventData.position);
    }
    public void OnSelect(BaseEventData baseEventData)
    {
        Debug.Log("OnSelect  on " + this.name + " : " + baseEventData);
    }
}
