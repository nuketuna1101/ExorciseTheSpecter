using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using UnityEngine.UIElements;
using System;

/// <summary>
/// ī�� ���� ��ü �����տ� �޷��ִ� ��ũ��Ʈ
/// </summary>
public class Card : MonoBehaviour
{
    [Header("Card : Data")]
    [SerializeField]
    private CardInfo _CardInfo; // ��� �ִ� ������
    public bool isFront;       // �յ޸� �÷���
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

    public void Setup(CardInfo _CardInfo, bool isFront)        // cardinfo �����Ϳ� ���� ������ ����ȭ  <<< ī�� Ÿ�԰� ���� Ŭ������ ���� ���� ��ȭ �߰� �ؾ���
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
    public void DotweenMove(PRS prs, float time = 0f)           // DOTWEEN �̿��Ͽ� ��ġ�� �̵�
    {
        transform.DOMove(prs.pos, time);
        transform.DORotateQuaternion(prs.rot, time);
        transform.DOScale(prs.scale, time);
    } 
    public void LocateCard(Vector3 pos, Quaternion rot, Vector3 scale)          // ī�� �ش� ���·� ��ġ��Ű��
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
        this.transform.localScale = scale;
    }   
    public void LocateCard(Vector3 pos)             // ī�� �ش� ���·� ��ġ��Ű��
    {
        this.transform.position = pos;
    }   

    //---------------------------------------------------------
    // rendering ����
    /// <summary>
    /// ī�� �������� ������ �ڵ�ȭ ���� ��ũ��Ʈ
    /// "Rendering Order ����"
    /// </summary>

    private const string _SortingLayerName = "Card";
    private int originOrder;

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }
    public void FocusAsMostFront()          // ���� ������ ���� �ٲٱ�
    {
        SetOrder(-100);
    }   
    public void RevertOrder()           // ���� ������ ���ư���
    {
        SetOrder(originOrder);
    }                        
    private void SetOrder(int order)            // �����տ� ����ִ� ������ ���� ����
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
    /// ��ġ �̺�Ʈ
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

    #region ī�� �ڽ�Ʈ�� ī�� ��� ����

    public int GetCardCost()
    {
        return this._CardInfo.CardCost;
    }
    #endregion
}
