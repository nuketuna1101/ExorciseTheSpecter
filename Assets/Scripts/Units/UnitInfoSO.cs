using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����ɷ� ����
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
