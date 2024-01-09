using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ī�� ����
/// </summary>
/// 

/// <summary>
/// ī�� ���� Ŭ���� ����ü
/// </summary>

[Serializable]
public class CardInfo
{
    private int Class;
    private int CardID;
    private int CardType;
    private int CardCost;
    private string CardName;
    private string CardDescription;
}

[CreateAssetMenu(fileName = "CardInfoSO", menuName = "Scriptable Object/CardInfoSO")]
public class CardInfoSO : ScriptableObject
{
    public CardInfo[] cardInfos;

}
