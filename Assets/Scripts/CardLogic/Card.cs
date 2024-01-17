using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using UnityEngine.UIElements;
using System;

/// <summary>
/// 카드 개별 객체 프리팹에 달려있는 스크립트
/// </summary>
public class Card : MonoBehaviour
{
    [Header("Card : Data")]
    [SerializeField]
    private CardInfo _CardInfo; // 담고 있는 데이터
    public bool isFront;       // 앞뒷면 플래그
    public PRS originalPRS;

    [Header("CardPrefab Setup Sprite and TMP")]
    [SerializeField]
    private TMP_Text text_cardCost;
    [SerializeField]
    private TMP_Text text_cardDescript;
    [SerializeField]
    private TMP_Text text_cardName; 
    [SerializeField]
    private TMP_Text text_cardType;
    [SerializeField]
    private GameObject img_cardType;
    private readonly string[] cardTypeText = { "Null", "Action", "Skill", "Suggest" };


    [Header("CardPrefab Rendering")]
    [SerializeField]
    private Renderer[] RenderOrder0;
    [SerializeField]
    private Renderer[] RenderOrder1;
    [SerializeField]
    private Renderer[] RenderOrder2;
    [SerializeField]
    private Renderer[] RenderOrder3;
    [SerializeField]
    private Renderer[] RenderOrder4;

    public void Setup(CardInfo _CardInfo, bool isFront)        // cardinfo 데이터에 따라서 프리팹 가시화  <<< 카드 타입과 직업 클래스에 따른 색상 변화 추가 해야함
    {
        this._CardInfo = _CardInfo;
        this.isFront = isFront;

        if (this.isFront)
        {
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(4).gameObject.SetActive(true);
            text_cardCost.text = this._CardInfo.CardCost.ToString();
            text_cardDescript.text = this._CardInfo.CardDescription.ToString();
            text_cardName.text = this._CardInfo.CardName.ToString();
            text_cardType.text = cardTypeText[this._CardInfo.CardType];
            img_cardType.GetComponent<SpriteRenderer>().color = ColorSettings.cardTypeColor[this._CardInfo.CardType];
        }
        else
        {
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
            text_cardCost.text = "";
            text_cardDescript.text = "";
            text_cardName.text = "";
            text_cardType.text = "";
        }
    }
    public void DotweenMove(PRS prs, float time = 0f)           // DOTWEEN 이용하여 위치로 이동
    {
        transform.DOMove(prs.pos, time);
        transform.DORotateQuaternion(prs.rot, time);
        transform.DOScale(prs.scale, time);
    } 
    public void LocateCard(Vector3 pos, Quaternion rot, Vector3 scale)          // 카드 해당 상태로 위치시키기
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
        this.transform.localScale = scale;
    }   
    public void LocateCard(Vector3 pos)             // 카드 해당 상태로 위치시키기
    {
        this.transform.position = pos;
    }   

    //---------------------------------------------------------
    // rendering 관련
    /// <summary>
    /// 카드 프리팹의 랜더링 자동화 위한 스크립트
    /// "Rendering Order 관리"
    /// </summary>

    private const string _SortingLayerName = "Card";
    private int originOrder;

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }
    public void FocusAsMostFront()          // 가장 앞으로 오더 바꾸기
    {
        SetOrder(-100);
    }   
    public void RevertOrder()           // 원래 오더로 돌아가기
    {
        SetOrder(originOrder);
    }                        
    private void SetOrder(int order)            // 프리팹에 들어있는 랜더링 오더 설정
    {
        int order_Criterion = order * 10 * (-1);
        foreach (var renderer in RenderOrder0)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = order_Criterion;
        }
        foreach (var renderer in RenderOrder1)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = order_Criterion + 1;
        }
        foreach (var renderer in RenderOrder2)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = order_Criterion + 2;
        }
        foreach (var renderer in RenderOrder3)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = order_Criterion + 3;
        }
        foreach (var renderer in RenderOrder4)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = order_Criterion + 4;
        }
    }          
    //--------------------------------------------
    /// <summary>
    /// 터치 이벤트
    /// </summary>
    
    void OnMouseOver()
    {
        if (isFront)
            CardManager.Instance.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        if (isFront)
            CardManager.Instance.CardMouseExit(this);
    }
    
    void OnMouseDown()
    {
        if (isFront)
            CardManager.Instance.CardMouseDown();
    }

    void OnMouseUp()
    {
        if (isFront)
            CardManager.Instance.CardMouseUp();
    }


    //--------------------------------------------------------------------------

    #region 카드 코스트와 카드 사용 관련

    public int GetCardCost()
    {
        return this._CardInfo.CardCost;
    }
    #endregion
}
