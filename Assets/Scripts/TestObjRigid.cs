using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObjRigid : MonoBehaviour,
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
        //var tmpPos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        //tmpPos.z = 0.0f;
        //this.transform.position = tmpPos;
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
    public void OnCollisionExit2D(Collision2D collision)
    {
        DebugOpt.Log("OnCollisionExit2D on " + this.name + " : " + collision.gameObject.name);

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        DebugOpt.Log("OnTriggerExit2D on " + this.name + " : " + collision.gameObject.name);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DebugOpt.Log("OnTriggerEnter2D on " + this.name + " : " + collision.gameObject.name);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DebugOpt.Log("OnCollisionEnter2D on " + this.name + " : " + collision.gameObject.name);

    }
}
