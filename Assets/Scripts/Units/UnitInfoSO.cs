using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 전투능력 정보
/// </summary>
/// 
[CreateAssetMenu(fileName = "UnitInfoSO", menuName = "Scriptable Object/UnitInfoSO")]
public class UnitInfoSO : ScriptableObject
{
    public int maxHp;
    public int curHP;
    public int Armor;
    public SpellAdaptability _SpellAdaptability;
    public int strength;
    public int intelligence;
}
