using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카드 정보
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
    public List<CardEffect> CardEffectList;
}

[Serializable]
public class CardEffect
{
    public int TypeCode;
    public int TargetType;
    public int EffectType;
    public int EffectAmount;
    public int EffectRepeat;
}




[CreateAssetMenu(fileName = "CardInfoSO", menuName = "Scriptable Object/CardInfoSO")]
public class CardInfoSO : ScriptableObject
{
    public List<CardInfo> CardInfoList;
}
