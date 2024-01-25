using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerGameDataSO", menuName = "Scriptable Object/PlayerGameDataSO")]
public class PlayerGameDataSO : ScriptableObject
{
    [Header("Character Data")]
    public int characterCode;
    public int maxHp;
    public int curHP;
    public int gold;
    public List<int> blessings;
    public int curse;
    public UnitInfo unitInfo;

    [Header("Map and Chamber Data")]
    public int currentStageNumber;
    public bool[] curVisited;
    public int curChamberInProgress;

    [Header("Card Data")]
    public List<CardInfo> myDeck;
}
