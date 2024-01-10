using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CardManager : Singleton<CardManager>
{

    // ��ü ��, ���� �ڵ�, ���� ī��, ����� ī����̿� ���� �������� ����

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
        // �׽�Ʈ�� �� �ʱ�ȭ
        myDeck = new List<CardInfo>(DataManager.Instance.TotalCardNumber);
    }


    //-------------------------------------------


    void SetupCardBuffer()
    {
        // ī�� ���� ä���
        cardBuffer = new List<CardInfo>();
        for (int i = 0; i < 12; i++)
        {
            var rand = Random.Range(0, DataManager.Instance.TotalCardNumber);
            cardBuffer.Add(DataManager.Instance._TempAccessCardInfoSO.CardInfoList[rand]);
        }
    }



    public CardInfo PopCard()
    {
        //
        if (cardBuffer.Count == 0)
            SetupCardBuffer();

        CardInfo cardinfo = cardBuffer[0];
        cardBuffer.RemoveAt(0);
        return cardinfo;
    }


    public void TestPop()
    {
        //
        if (cardBuffer.Count == 0)
            SetupCardBuffer();

        DebugOpt.Log("POP :: " + cardBuffer[0].CardName);

        CardInfo cardinfo = cardBuffer[0];
        cardBuffer.RemoveAt(0);
    }



    public void AddCardToMyHand()
    {
        // Ǯ������ ��ü
        var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        //card.Setup(PopCard(), isMine);
        myCards.Add(card);

        SetOriginOrder();
        CardAlignment();
    }

    // ī�� ����
    private void CardAlignment()
    {
        DebugOpt.Log("+-- : CardAlignment called");
        var prsList = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 1.9f);
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

            targetCard.originalPRS = prsList[i];
            targetCard.MoveTransform(targetCard.originalPRS, true, 0.7f);
        }
    }


    private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        DebugOpt.Log("+-- : RoundAlignment called");
        //
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);
        //
        switch (objCount)
        {
            case 1:
                objLerps = new float[] { 0.5f };
                break;
            case 2:
                objLerps = new float[] { 0.27f, 0.73f };
                break;
            case 3:
                objLerps = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            default:
                float interval = 1.0f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }
        //
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            //
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);   
            }
            //
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }


    private void DrawCard()
    {
        // ���з� ī�� �߰�, ����ȭ
    }


    private void SetOriginOrder()
    {
        
        // ���� ����
        int cnt = myCards.Count;
        for (int i = 0; i < cnt; i++)
        {
            var targetCard = myCards[i];
            //targetCard?.GetComponent<CardOrder>().SetOriginOrder(i);
            targetCard?.GetComponent<Card>().SetOriginOrder(i);
        }

    }

}
