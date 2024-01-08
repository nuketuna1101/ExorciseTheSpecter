using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestCardManager : Singleton<TestCardManager>
{
    [SerializeField]
    private CardInfo cardInfo;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private List<CardInfo> myDeck;

    private Queue<CardInfo> ReadyQueue;
    private List<CardInfo> hand;
    private List<CardInfo> used;

    [SerializeField]
    private Transform cardSpawnPoint;



    void AddCard()
    {

    }

    void SetOriginOrder()
    {

    }

    void CardAlignment()
    {
    }

    List<Transform> RoundAlignment(Transform leftTransform, Transform rightTransform, int ObjCount, float height, Vector3 scale)
    {
        return null;
    }
}
