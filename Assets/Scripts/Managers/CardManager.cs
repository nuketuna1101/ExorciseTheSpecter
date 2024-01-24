using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
/// <summary>
/// 전체 덱, 손패 핸드, 뽑을 카드, 사용한 카드더미에 대한 전반적인 관리
/// </summary>
public class CardManager : Singleton<CardManager>
{
    [SerializeField]    private CardInfoSO _CardInfoSO;         // 데이터 매니저의 전체 카드 사전
    // 실제 플레이어의 덱
    private List<CardInfo> myDeck;
    // 
    [Header("Card Data Structures: ReadyQueue, myCard(hand), usedCards")]
    [SerializeField] private List<CardInfo> cardBuffer;      // TEST LEGACY CODE
    [SerializeField] private Queue<CardInfo> ReadyQueue;     // 뽑을 카드 더미 덱
    [SerializeField] private List<Card> myCards;             // 손패에 들고있는 카드들
    [SerializeField] private List<CardInfo> usedCards;             // 손패에 들고있는 카드들

    [SerializeField] private GameObject cardPrefab;          // 카드 프리팹 생성하므로
    [SerializeField] private Transform myCardLeft;           // 손패 정리위한 위치
    [SerializeField] private Transform myCardRight;          // 손패 정리위한 위치

    // 덱 ui
    [Header("Deck : to ")]
    [SerializeField] private Transform cardSpawnPoint;          // 뽑을 카드더미 덱 위치.
    [SerializeField] public Transform cardRecallPoint;          // 뽑을 카드더미 덱 위치.
    [SerializeField] private TMP_Text remainCount;              // 뽑을 카드더미 남은 카드숫자


    private readonly WaitForSeconds wfs25 = new WaitForSeconds(0.25f);


    //-------------------------------------------
    // 최종 코드

    #region 카드 드로우 및 손패 관련
    public void DrawCards(int amount)                       // 수량만큼 덱에서 핸드로 카드 드로우.. 순차드로우 효과 위해 코루틴
    {
        StartCoroutine(DrawCardsCor(amount));
    }
    private IEnumerator DrawCardsCor(int amount)            //
    {
        WaitForSeconds wtf = new WaitForSeconds(0.25f);
        for (int i = 0; i < amount; i++)
        {
            yield return wtf;
            DrawCardFromDeckToHand();
        }
    }
    private void DrawCardFromDeckToHand()        // 뽑을 카드더미 덱에서 손패로 카드 1장 드로우
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
        card.Setup(ReadyQueue.Dequeue(), true);
        card.LocateCard(cardSpawnPoint.position);
        myCards.Add(card);
        // 오디오
        AudioManager.Instance.PlaySFX(SFX_TYPE.CARD_DRAW);

        // 랜더링과 손패 가시화 정리
        SetHandCardsOrdered();
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
            myCards[i].DotweenMove(myCards[i].originalPRS, 0.7f);
        }
    }
    private void SetHandCardsOrdered()       // 손패 카드 랜더링
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<Card>().SetOriginOrder(i);
        }
    }
    #endregion
    //-------------------------------------------
    // TEST CODE

    public void TestInitDeck()
    {
        // 테스트용으로 나의 전체 덱 설정해주기 임의로
        int decksize = 15;              // 임의 덱 사이즈
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);

        // 핸드 초기화, 사용한 카드 초기화
        myCards.Clear();
        usedCards.Clear();
        //
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 15; i++)
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
    #region 카드 조작 관련
    public void CardPointerDown(Card card)                      // 카드 누를시 : 카드 내용 볼 수 있도록 정렬, 확대, 맨앞 랜더링
    {
        EnlargeCard(card);
    }

    public void CardPointerUp(Card card, bool isOnHandArea)     // 카드 떼어내면, 어디에 있느냐에 따라.
    {
        if (isOnHandArea)
        {
            RevertEnlargeCard(card);        // 다시 돌아가기
        }
        else
        {
            TryUsingCard(card);             // 사용 시도
        }
    }
    #endregion

    private const float EnlargeCoeff = 1.5f;            // 확대 배수 조정값
    private void EnlargeCard(Card card)                 // 카드 잘 보기 위해 정렬, 확대, 맨 앞 랜더링
    {
        Vector3 pos = new Vector3(card.originalPRS.pos.x, -2.0f, 10f);           // 확대될 때의 위치 조정값
        card.LocateCard(pos, Quaternion.identity, Vector3.one * EnlargeCoeff);
        card.GetComponent<Card>().FocusAsMostFront();
    }
    private void RevertEnlargeCard(Card card)           // 카드 원래 위치로 돌아가기
    {
        card.LocateCard(card.originalPRS.pos, card.originalPRS.rot, card.originalPRS.scale);
        card.GetComponent<Card>().RevertOrder();
    }


    private bool IsAvailableCard(Card card)                // 해당카드가 사용가능한지 에너지 코스트 판단
    {
        return GameManager.Instance.GetEnergy() >= card.GetCardCost();
    }

    private void ActivateCard(Card card)                   // 카드가 사용됨 : 카드 회수 작업, 카드 효과 진행
    {
        // 카드 효과
        /**/
        // 카드 데이터와 프리팹 회수, 에너지 소모
        //_Card.transform.DOKill();
        //GameManager.Instance.ConsumeEnergy(_Card.GetCardCost());
        //PoolManager.ReturnToPool(_Card.gameObject);
        GameManager.Instance.ConsumeEnergy(card.GetCardCost());
        DiscardCard(card);
    }
    private void DiscardCard(Card card)                    // 프리팹 회수
    {
        StartCoroutine(DiscardCardCor(card));
    }

    private IEnumerator DiscardCardCor(Card card)
    {
        AudioManager.Instance.PlaySFX(SFX_TYPE.CARD_DRAW);
        card.transform.DOMove(cardRecallPoint.position, 0.25f);
        yield return wfs25;
        card.transform.DOKill();
        PoolManager.ReturnToPool(card.gameObject);
    }
    public void ClearHand()                                 // 손패에 있던 모든 카드 회수.
    {
        StartCoroutine(ClearHandCor());
    }
    private IEnumerator ClearHandCor()
    {
        while (myCards.Count > 0)
        {
            yield return wfs25;
            var tmpCard = myCards[0];
            usedCards.Add(tmpCard.GetCardInfo());
            myCards.Remove(tmpCard);
            DiscardCard(tmpCard);
        }
    }
    public int GetReadyQueueSize()                          // 뽑을카드더미=
    {
        if (ReadyQueue == null)
        {
            DebugOpt.Log("ReadyQueue Null Checking :: ISNULL");
            return -1;
        }
        return ReadyQueue.Count;
    }

    private void TryUsingCard(Card card)                 // 코스트를 소모하여 카드 사용효과
    {
        if (IsAvailableCard(card))            // 
        {
            // 핸드에서 제거
            usedCards.Add(card.GetCardInfo());
            myCards.Remove(card);
            ActivateCard(card);
            AlignHandCards();
        }
        else            // 카드 소모 실패
        {
            AudioManager.Instance.PlaySFX(SFX_TYPE.FAIL);
            UIManager.Instance.Popup_NotifyWindow_Warn();
            myCards.ForEach(x => x.GetComponent<Card>().RevertOrder());
            AlignHandCards();
        }
    }
}