using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class CardManager : Singleton<CardManager>
{

    // 전체 덱, 손패 핸드, 뽑을 카드, 사용한 카드더미에 대한 전반적인 관리

    [SerializeField]
    private CardInfoSO _CardInfoSO;

    [SerializeField]
    private List<CardInfo> hand;


    [SerializeField]
    private Transform cardSpawnPoint;

    //

    private List<CardInfo> myDeck;
    // 
    private List<CardInfo> cards_TobeUsed;
    private List<CardInfo> cards_Hand;
    private List<CardInfo> cards_AlreadyUsed;


    [SerializeField]
    List<CardInfo> cardBuffer;

    [SerializeField]
    GameObject cardPrefab; 
    [SerializeField]
    List<Card> myCards;
    [SerializeField]
    Transform myCardLeft;
    [SerializeField]
    Transform myCardRight;



    //-------------------------------------------
    private void InitDeck()
    {
        // 테스트용 덱 초기화
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);
    }
    private void DrawCard()
    {
        // FROM 덱 TO 손패

    }

    //-------------------------------------------


    void SetupCardBuffer()// LEGACY TEST CODE
    {
        // 카드 버퍼 채우기
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 12; i++)
        {
            var rand = UnityEngine.Random.Range(0, DataManager.Instance.TotalCardNumber);
            cardBuffer.Add(DataManager.Instance._TempAccessCardInfoSO.CardInfoList[rand]);
        }
    }

    public CardInfo PopCard()// LEGACY TEST CODE
    {
        //
        if (cardBuffer.Count == 0)
            SetupCardBuffer();

        CardInfo cardinfo = cardBuffer[0];
        cardBuffer.RemoveAt(0);
        return cardinfo;
    }

    public void TestPop()// LEGACY TEST CODE
    {
        //
        if (cardBuffer.Count == 0)
            SetupCardBuffer();

        DebugOpt.Log("POP :: " + cardBuffer[0].CardName);

        CardInfo cardinfo = cardBuffer[0];
        cardBuffer.RemoveAt(0);
    }

    public void AddCardToMyHand()       // LEGACY TEST CODE
    {
        // 풀링으로 대체
        var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        myCards.Add(card);

        SetOriginOrder();
        AlignHandCards();
    }

    private void AlignHandCards()            // 손패 카드 정렬 .. 선형 보간으로
    {       
        var scale = Vector3.one;
        float[] objLerps = new float[myCards.Count];

        for (int i = 0; i < myCards.Count; i++)
        {
            // 싱글 카드 경우만 예외.
            if (myCards.Count == 1)
                objLerps[i] = 0.5f;
            else
                objLerps[i] = 1.0f / (myCards.Count - 1) * i;

            var targetPos = Vector3.Lerp(myCardLeft.position, myCardRight.position, objLerps[i]);
            var targetRot = Quaternion.Slerp(myCardLeft.rotation, myCardRight.rotation, objLerps[i]);
            myCards[i].originalPRS = new PRS(targetPos, targetRot, scale);
            myCards[i].MoveTransform(myCards[i].originalPRS, true, 0.7f);
        }
    }

    private void SetOriginOrder()       // 손패 카드 랜더링
    {     
        // 오더 정렬
        int cnt = myCards.Count;
        for (int i = 0; i < cnt; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Card>().SetOriginOrder(i);
        }
    }

    //--------------------------------------------------

    /*
    Card selectCard;
    bool isMyCardDrag;
    bool onMyCardArea;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    int myPutCount;


    void Start()
    {
        //SetupItemBuffer();
        //TurnManager.OnAddCard += AddCard;
        //TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void OnDestroy()
    {
        //TurnManager.OnAddCard -= AddCard;
        //TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void OnTurnStarted(bool myTurn)
    {
        //if (myTurn)
            //myPutCount = 0;
    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }

    public bool TryPutCard(bool isMine)
    {
        if (isMine && myPutCount >= 1)
            return false;

        if (!isMine && otherCards.Count <= 0)
            return false;

        //Card card = isMine ? selectCard : otherCards[Random.Range(0, otherCards.Count)];
        //var spawnPos = isMine ? Utils.MousePos : otherCardSpawnPoint.position;
        //var targetCards = isMine ? myCards : otherCards;

        Card card = selectCard;
        var spawnPos = isMine ? Utils.MousePos : otherCardSpawnPoint.position;
        var targetCards = isMine ? myCards : otherCards;

        if (EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos))
        {
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            if (isMine)
            {
                selectCard = null;
                myPutCount++;
            }
            CardAlignment(isMine);
            return true;
        }
        else
        {
            targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
            CardAlignment(isMine);
            return false;
        }
    }


    #region MyCard

    public void CardMouseOver(Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;

        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;

        if (eCardState != ECardState.CanMouseDrag)
            return;

        if (onMyCardArea)
            EntityManager.Inst.RemoveMyEmptyEntity();
        else
            TryPutCard(true);
    }

    void CardDrag()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            EntityManager.Inst.InsertMyEmptyEntity(Utils.MousePos.x);
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -4.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 3.5f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.Nothing;

        else if (!TurnManager.Inst.myTurn || myPutCount == 1 || EntityManager.Inst.IsFullMyEntities)
            eCardState = ECardState.CanMouseOver;

        else if (TurnManager.Inst.myTurn && myPutCount == 0)
            eCardState = ECardState.CanMouseDrag;
    }

    #endregion
    */
}
