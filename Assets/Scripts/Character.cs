using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle Scene���� ���� ��ȣ�ۿ뿡 �ʿ��� ��ü Ŭ������ �޼ҵ� ����
/// </summary>

public abstract class BattleObj
{
    // ���� ��ü�� ������ �� �޼ҵ�
    // �ൿ, ����, �����
    // �޼ҵ� �߻� ����? : �� ����, �� �߰�, �� ����
    [Header("BattleObj : Battle Data")]
    // ü��, ��, �ֹ�������
    public int maxHp = 400;
    public int curHP = 400;
    public int Armor = 16;
    public SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�
    public int strength = 0;
    public int intelligence = 0;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    // �����̻� ����
    private StatusEffectArray _StatusEffectArray = new StatusEffectArray(0);
    public Dictionary<StatusEffectType, int> StatusEffectDict = new Dictionary<StatusEffectType, int>(5);


    /// <summary>
    /// ���� ���� �����ؼ� ��Ÿ ���� ȿ���� ���� ���Ŀ� �ݿ����� �ʾ��� ���� �ʿ�
    /// </summary>
    /// <param name="_Object"></param>
    /// <param name="_DamageType"></param>
    /// <param name="_DamageValue"></param>

    public void Attack(BattleObj _Object, DamageType _DamageType, int _DamageValue)
    {
        DebugOpt.Log("method Attack called from  " + this);
        int CalculatedDamageValue = _DamageValue;
        if (_DamageType == DamageType.Physical)
        {
            // ����ŭ �߰� ������, �����̻�'Exhausted' �� ������ �氨
            CalculatedDamageValue += this.strength;

        }
        else if (_DamageType == DamageType.Magical)
        {
            // ���ɸ�ŭ �߰� ������, �����̻�'Dizzy' �� ������ �氨
            CalculatedDamageValue += this.intelligence;
        }
        _Object.BeAttacked(_DamageType, CalculatedDamageValue);
    }
    private void BeAttacked(DamageType _DamageType, int CalculatedDamageValue)
    {
        // ���� ��������ŭ �ǰ�
        DebugOpt.Log("method BeAttacked called from  " + this);
        switch (_DamageType)
        {
            case DamageType.Physical:
                break;
            case DamageType.Magical:
                switch (_SpellAdaptability)
                {
                    case SpellAdaptability.None:
                        break;
                    case SpellAdaptability.Resist:
                        CalculatedDamageValue -= (CalculatedDamageValue / 4);
                        break;
                    case SpellAdaptability.Immune:
                        CalculatedDamageValue = 0;
                        break;
                    default:
                        break;
                }
                break;
            case DamageType.TrueDamage:
                break;
        }

        this.curHP -= CalculatedDamageValue;


        // ���� ü�� 0 ���ϸ� ���ó��
    }
    public void GetArmorReduced(int value)
    {
        // �� ���� ����
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        this.Armor = (this.Armor >= value ? this.Armor - value : 0);
    }
    public void GiveStatusEffect(BattleObj _Object, StatusEffect _StatusEffect)
    {
        // ��뿡�� ���� �̻� ȿ���� ���� �ϸ�ŭ �ο�
        _Object.GetStatusEffect(_StatusEffect);
    }
    private void GetStatusEffect(StatusEffect _StatusEffect)
    {
        // �����̻� ȿ�� ����
        // ����: ��ġ�� ���뿡 ���� �����ϻ� ���� ȿ�� ������ ���߿�
        _StatusEffectArray.AddValue(_StatusEffect);
    }

    public void GetEffectWhenTurnStarts()
    {
        // �� ���� �� �޴� ȿ�� �ߵ�
        // ȿ�� ť�� �־ ����




    }
    public void GetEffectWhenTurnEnds()
    {
        // �� ���� �� �޴� ȿ�� �ߵ�

        // ����, �ߵ��� �� ���� �� �ߵ���
    }

    public void LogMyStatsForTest()
    {
        DebugOpt.Log(this + " :: my stats : curHP : " + this.curHP + " , armor : " + this.Armor);
    }
}


public class Player : BattleObj
{

}

public class Enemy : BattleObj
{

}

/*
public class Player : BattleObj
{
    [Header("Player : Battle Data")]
    // ü��, ��, �ֹ�������
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�, ħ��
    private int strength;
    private int intelligence;
    private int composure;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    // �ູ, ����
    private List<int> Blessings;
    private int Curses;
}

public class Enemy : BattleObj
{
    [Header("Enemy: Indentity")]
    private int EnemyID;

    [Header("Enemy : Battle Data")]
    // ü��, ��, �ֹ�������
    private int maxHp = 100;
    private int curHP = 100;
    private int Armor = 25;
    private SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�
    private int strength = 0;
    private int intelligence = 0;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    //
    private List<int> ActionList;
}



public class BattleJudge
{
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    //

    public void HitDmg(BattleObj _Subject, BattleObj _Object, int _DmgAmount, int _DmgType)
    {

    }

}
*/