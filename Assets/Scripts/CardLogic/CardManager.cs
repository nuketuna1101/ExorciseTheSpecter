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
            //myCards[i].MoveTransform(myCards[i].originalPRS, true, 0.7f);
            myCards[i].DotweenMove(myCards[i].originalPRS, 0.7f);
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

    private Card selectCard;                            // 현재 선택한 카드객체
    public void CardMouseOver(Card card)
    {
        selectCard = card;
        EnlargeCard(card);
    }
    public void CardMouseExit(Card card)
    {
        RevertEnlargeCard(card);
    }
  
    private const float EnlargeCoeff = 1.5f;            // 확대 배수 조정값
    private void EnlargeCard(Card card)
    {
        Vector3 pos = new Vector3(card.originalPRS.pos.x, -2.0f, 10f);           // 확대될 때의 위치 조정값
        card.LocateCard(pos, Quaternion.identity, Vector3.one * EnlargeCoeff);
        card.GetComponent<Card>().SetMostFrontOrder();
    }
    private void RevertEnlargeCard(Card card)
    {
        card.LocateCard(card.originalPRS.pos, card.originalPRS.rot, card.originalPRS.scale);
        card.GetComponent<Card>().RevertOrder();
    }
    //-------------------------------------------------------------

    private bool onMyCardArea;

    public void CardDrag()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = -5f;
        selectCard.LocateCard(touchPos, Quaternion.identity, selectCard.originalPRS.scale);
    }

    void DetectCardArea()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = -5f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(touchPos, Vector3.forward);
        int layer = LayerMask.NameToLayer("HandArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    private void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
    }

    
    bool isMyCardDrag;
    public void CardMouseDown()
    {
        isMyCardDrag = true;
    }
    public void CardMouseUp()
    {
        isMyCardDrag = false;
    }
}