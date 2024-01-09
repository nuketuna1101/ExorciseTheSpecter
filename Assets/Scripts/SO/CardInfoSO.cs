using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카드 정보
/// </summary>
/// 

/// <summary>
/// 카드 관련 클래스 구조체
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
