using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitInfo
{
    public int HP;
    public int Armor;
    public SpellAdaptability _SpellAdaptability;
    public int strength;
    public int intelligence;
}
[Serializable]
public class EnemyWiki
{
    public int StageNumber;
    public int EnemyCode;
    public string EnemyName;
    public int isBoss;
    public UnitInfo unitInfo;
}

[CreateAssetMenu(fileName = "EnemyWikiSO", menuName = "Scriptable Object/EnemyWikiSO")]
public class EnemyWikiSO : ScriptableObject
{
    public List<EnemyWiki> EnemyWikiList;
}
