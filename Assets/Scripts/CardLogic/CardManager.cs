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

    [SerializeField] private List<CardInfo> cardBuffer;      // TEST LEGACY CODE
    [SerializeField] private Queue<CardInfo> ReadyQueue;     // ���� ī�� ���� ��

    [SerializeField] private GameObject cardPrefab;          // ī�� ������ �����ϹǷ�
    [SerializeField] private List<Card> myCards;             // ���п� ����ִ� ī���
    [SerializeField] private Transform myCardLeft;           // ���� �������� ��ġ
    [SerializeField] private Transform myCardRight;          // ���� �������� ��ġ

    // �� ui
    [Header("Deck : to ")]
    [SerializeField] private Transform cardSpawnPoint;          // ���� ī����� �� ��ġ.
    [SerializeField] public Transform cardRecallPoint;          // ���� ī����� �� ��ġ.
    [SerializeField] private TMP_Text remainCount;              // ���� ī����� ���� ī�����



    //-------------------------------------------
    // ���� �ڵ�
    public void DrawCardFromDeckToHand()        // ���� ī����� ������ ���з� ī�� 1�� ��ο�
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
        UpdateDeckCardAmount();
        card.LocateCard(cardSpawnPoint.position);
        myCards.Add(card);
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

    //-------------------------------------------
    // TEST CODE

    public void TestInitDeck()
    {
        // �׽�Ʈ������ ���� ��ü �� �������ֱ� ���Ƿ�
        int decksize = 15;              // ���� �� ������
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);

        // �ڵ� �ʱ�ȭ
        myCards.Clear();

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
            UpdateDeckCardAmount();
        }

    }
    
    private void UpdateDeckCardAmount()          // ���� ī����� ���� ������Ʈ
    {
        //remainCount.text = ReadyQueue.Count.ToString();
    }


    public void InitReadyDeck()
    {
        // ���� ī�� ���� �ʱ�ȭ
        // ó�� ������ �� ���� ��ü ������ �ʱ�ȭ       
    }

    public void SetupCardBuffer()// LEGACY TEST CODE
    {
        // ī�� ���� ä���
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 5; i++)
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
    [SerializeField]
    private Card selectCard;                            // ���� ������ ī�尴ü
    public void CardMouseOver(Card card)
    {
        selectCard = card;
        EnlargeCard(card);
    }
    public void CardMouseExit(Card card)
    {
        RevertEnlargeCard(card);
    }
  
    private const float EnlargeCoeff = 1.5f;            // Ȯ�� ��� ������
    private void EnlargeCard(Card card)
    {
        Vector3 pos = new Vector3(card.originalPRS.pos.x, -2.0f, 10f);           // Ȯ��� ���� ��ġ ������
        card.LocateCard(pos, Quaternion.identity, Vector3.one * EnlargeCoeff);
        card.GetComponent<Card>().FocusAsMostFront();
    }
    private void RevertEnlargeCard(Card card)
    {
        card.LocateCard(card.originalPRS.pos, card.originalPRS.rot, card.originalPRS.scale);
        card.GetComponent<Card>().RevertOrder();
    }
    //-------------------------------------------------------------
    // �����丵 �� �κ�
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

        // ���� ���� ������ TryUsingCard ����

        if (!onMyCardArea)
            TryUsingCard();

    }



    private void TryUsingCard()                 // �ڽ�Ʈ�� �Ҹ��Ͽ� ī�� ���ȿ��
    {
        if (selectCard.IsAvailable())
        {
            // �ڵ忡�� ����
            myCards.Remove(selectCard);
            selectCard.ActivateCard();
            selectCard = null;
            AlignHandCards();
        }
        else
        {
            AudioManager.Instance.PlaySFX(SFX_TYPE.FAIL);
            myCards.ForEach(x => x.GetComponent<Card>().RevertOrder());
            AlignHandCards();
        }



        /*
        // 1�� �÷ο� : ī�� ��� ����
        // �ش� ī�带 ����
        myCards.Remove(selectCard);
        selectCard.transform.DOKill();
        PoolManager.ReturnToPool(selectCard.gameObject);
        selectCard = null;
        AlignHandCards();


        // 2�� �÷ο� : ī�� ��� ����, �ٽ� �ڵ�� ���ư�
        myCards.ForEach(x => x.GetComponent<Card>().RevertOrder());
        AlignHandCards();

        */
    }



    public bool IsAvailableCard()               // �ش�ī�尡 ��밡������ ������ �ڽ�Ʈ �Ǵ�
    {
        //



        return true;
    }

}