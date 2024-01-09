using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ī�� ����
/// </summary>
/// 
[Serializable]
public class CardInfo
{
    public int Class;
    public int CardID;
    public int CardType;
    public int CardCost;
    public string CardName;
    public string CardDescription;
}

[CreateAssetMenu(fileName = "CardInfoSO", menuName = "Scriptable Object/CardInfoSO")]
public class CardInfoSO : ScriptableObject
{
    public List<CardInfo> CardInfoList;
}
