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
/// ��ü ��, ���� �ڵ�, ���� ī��, ����� ī����̿� ���� �������� ����
/// </summary>
public class CardManager : Singleton<CardManager>
{
    [SerializeField]    private CardInfoSO _CardInfoSO;         // ������ �Ŵ����� ��ü ī�� ����
    // ���� �÷��̾��� ��
    private List<CardInfo> myDeck;
    // 
    [Header("Card Data Structures: ReadyQueue, myCard(hand), usedCards")]
    [SerializeField] private List<CardInfo> cardBuffer;      // TEST LEGACY CODE
    [SerializeField] private Queue<CardInfo> ReadyQueue;     // ���� ī�� ���� ��
    [SerializeField] private List<Card> myCards;             // ���п� ����ִ� ī���
    [SerializeField] private List<CardInfo> usedCards;             // ���п� ����ִ� ī���

    [SerializeField] private GameObject cardPrefab;          // ī�� ������ �����ϹǷ�
    [SerializeField] private Transform myCardLeft;           // ���� �������� ��ġ
    [SerializeField] private Transform myCardRight;          // ���� �������� ��ġ

    // �� ui
    [Header("Deck : to ")]
    [SerializeField] private Transform cardSpawnPoint;          // ���� ī����� �� ��ġ.
    [SerializeField] public Transform cardRecallPoint;          // ���� ī����� �� ��ġ.
    [SerializeField] private TMP_Text remainCount;              // ���� ī����� ���� ī�����


    private readonly WaitForSeconds wfs25 = new WaitForSeconds(0.25f);


    //-------------------------------------------
    // ���� �ڵ�

    #region ī�� ��ο� �� ���� ����
    public void DrawCards(int amount)                       // ������ŭ ������ �ڵ�� ī�� ��ο�.. ������ο� ȿ�� ���� �ڷ�ƾ
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
    private void DrawCardFromDeckToHand()        // ���� ī����� ������ ���з� ī�� 1�� ��ο�
    {
        // ���ۿ� �����ðž����� �ȵſ�
        if (ReadyQueue.Count == 0)
        {
            DebugOpt.Log("�غ�ť�� ����־ �ȵſ�");
            return;
        }

        // ready deck���� hand�� ī�� ��ο�
        var cardObject = PoolManager.GetFromPool();
        Card card = cardObject.GetComponent<Card>();
        card.Setup(ReadyQueue.Dequeue(), true);
        card.LocateCard(cardSpawnPoint.position);
        myCards.Add(card);
        // �����
        AudioManager.Instance.PlaySFX(SFX_TYPE.CARD_DRAW);

        // �������� ���� ����ȭ ����
        SetHandCardsOrdered();
        AlignHandCards();
    }

    private void AlignHandCards()            // ���� ī�� ���� .. ���� ��������
    {
        var scale = Vector3.one;
        float[] objLerps = new float[myCards.Count];

        for (int i = 0; i < myCards.Count; i++)
        {
            // �̱� ī�� ��츸 ����.
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
    private void SetHandCardsOrdered()       // ���� ī�� ������
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
        // �׽�Ʈ������ ���� ��ü �� �������ֱ� ���Ƿ�
        int decksize = 15;              // ���� �� ������
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);

        // �ڵ� �ʱ�ȭ, ����� ī�� �ʱ�ȭ
        myCards.Clear();
        usedCards.Clear();
        //
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 15; i++)
        {
            var rand = UnityEngine.Random.Range(0, DataManager.Instance.TotalCardNumber);
            cardBuffer.Add(DataManager.Instance._TempAccessCardInfoSO.CardInfoList[rand]);
        }
        // �׽�Ʈ������ ���� ����Ʈ�� ť �״��
        ReadyQueue = new Queue<CardInfo>();
        for (int i = 0; i < cardBuffer.Count; i++)
        {
            ReadyQueue.Enqueue(cardBuffer[i]);
        }

    }

    //--------------------------------------------------
    #region ī�� ���� ����
    public void CardPointerDown(Card card)                      // ī�� ������ : ī�� ���� �� �� �ֵ��� ����, Ȯ��, �Ǿ� ������
    {
        EnlargeCard(card);
    }

    public void CardPointerUp(Card card, bool isOnHandArea)     // ī�� �����, ��� �ִ��Ŀ� ����.
    {
        if (isOnHandArea)
        {
            RevertEnlargeCard(card);        // �ٽ� ���ư���
        }
        else
        {
            TryUsingCard(card);             // ��� �õ�
        }
    }
    #endregion

    private const float EnlargeCoeff = 1.5f;            // Ȯ�� ��� ������
    private void EnlargeCard(Card card)                 // ī�� �� ���� ���� ����, Ȯ��, �� �� ������
    {
        Vector3 pos = new Vector3(card.originalPRS.pos.x, -2.0f, 10f);           // Ȯ��� ���� ��ġ ������
        card.LocateCard(pos, Quaternion.identity, Vector3.one * EnlargeCoeff);
        card.GetComponent<Card>().FocusAsMostFront();
    }
    private void RevertEnlargeCard(Card card)           // ī�� ���� ��ġ�� ���ư���
    {
        card.LocateCard(card.originalPRS.pos, card.originalPRS.rot, card.originalPRS.scale);
        card.GetComponent<Card>().RevertOrder();
    }


    private bool IsAvailableCard(Card card)                // �ش�ī�尡 ��밡������ ������ �ڽ�Ʈ �Ǵ�
    {
        return GameManager.Instance.GetEnergy() >= card.GetCardCost();
    }

    private void ActivateCard(Card card)                   // ī�尡 ���� : ī�� ȸ�� �۾�, ī�� ȿ�� ����
    {
        // ī�� ȿ��
        /**/
        // ī�� �����Ϳ� ������ ȸ��, ������ �Ҹ�
        //_Card.transform.DOKill();
        //GameManager.Instance.ConsumeEnergy(_Card.GetCardCost());
        //PoolManager.ReturnToPool(_Card.gameObject);
        GameManager.Instance.ConsumeEnergy(card.GetCardCost());
        DiscardCard(card);
    }
    private void DiscardCard(Card card)                    // ������ ȸ��
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
    public void ClearHand()                                 // ���п� �ִ� ��� ī�� ȸ��.
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
    public int GetReadyQueueSize()                          // ����ī�����=
    {
        if (ReadyQueue == null)
        {
            DebugOpt.Log("ReadyQueue Null Checking :: ISNULL");
            return -1;
        }
        return ReadyQueue.Count;
    }

    private void TryUsingCard(Card card)                 // �ڽ�Ʈ�� �Ҹ��Ͽ� ī�� ���ȿ��
    {
        if (IsAvailableCard(card))            // 
        {
            // �ڵ忡�� ����
            usedCards.Add(card.GetCardInfo());
            myCards.Remove(card);
            ActivateCard(card);
            AlignHandCards();
        }
        else            // ī�� �Ҹ� ����
        {
            AudioManager.Instance.PlaySFX(SFX_TYPE.FAIL);
            UIManager.Instance.Popup_NotifyWindow_Warn();
            myCards.ForEach(x => x.GetComponent<Card>().RevertOrder());
            AlignHandCards();
        }
    }
}