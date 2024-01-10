using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

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
    Queue<CardInfo> ReadyQueue;     // 뽑을 카드 더미 덱


    [SerializeField]
    GameObject cardPrefab; 
    [SerializeField]
    List<Card> myCards;             // 손패에 들고있는 카드들
    [SerializeField]
    Transform myCardLeft;
    [SerializeField]
    Transform myCardRight;



    //-------------------------------------------
    // 최종 코드
    private void InitDeck()
    {
        // 테스트용 덱 초기화
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);
    }
    public void DrawCardFromDeckToHand()        // 뽑을 카드더미 덱에서 손패로 카드 1장 드로우
    {
        // 버퍼에 꺼내올거없으면 안돼요
        if (ReadyQueue.Count == 0)
        {
            DebugOpt.Log("준비큐가 비어있어서 안돼요");
            return;
        }

        // ready deck에서 hand로 카드 드로우
        var cardObject = PoolManager.GetFromPool();
        Card card = cardObject.GetComponent<Card>();
        //card.Setup(PopCard(), true);            // 데이터 바인드
        card.Setup(ReadyQueue.Dequeue(), true);
        myCards.Add(card);
        // 랜더링과 손패 가시화 정리
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
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Card>().SetOriginOrder(i);
        }
    }

    //-------------------------------------------
    // TEST CODE

    public void TestInitDeck()
    {
        // 테스트용으로 나의 전체 덱 설정해주기 임의로
        int decksize = 15;
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);

    }

    public void InitReadyDeck()
    {
        // 뽑을 카드 더미 초기화
        // 처음 시작할 땐 나의 전체 덱에서 초기화
        
    }

    public void SetupCardBuffer()// LEGACY TEST CODE
    {
        // 카드 버퍼 채우기
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 5; i++)
        {
            var rand = UnityEngine.Random.Range(0, DataManager.Instance.TotalCardNumber);
            cardBuffer.Add(DataManager.Instance._TempAccessCardInfoSO.CardInfoList[rand]);
        }

        // 테스트용으로 버퍼 리스트를 큐 그대로
        ReadyQueue = new Queue<CardInfo>();
        for (int i = 0; i < cardBuffer.Count; i++)
        {
            ReadyQueue.Enqueue(cardBuffer[i]);
        }
    }

    //--------------------------------------------------

    Card selectCard;

    public void CardMouseOver(Card card)
    {
        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            //Vector3 enlargePos = new Vector3(card.originalPRS.pos.x, -4.8f, 10f);
            Vector3 enlargePos = new Vector3(card.originalPRS.pos.x, 0.0f, 10f);
            card.MoveTransform(new PRS(enlargePos, Quaternion.identity, Vector3.one * 1.5f), false);
        }
        else
            card.MoveTransform(card.originalPRS, false);

        card.GetComponent<Card>().SetMostFrontOrder(isEnlarge);
    }


    bool isMyCardDrag;
    bool onMyCardArea;

    public void CardMouseDown()
    {
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
    }











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
