using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle Scene���� ���� ��ȣ�ۿ뿡 �ʿ��� ��ü Ŭ������ �޼ҵ� ����
/// </summary>

public class BattleObj
{
    // ���� ��ü�� ������ �� �޼ҵ�
    // �ൿ, ����, �����
    // �޼ҵ� �߻� ����? : �� ����, �� �߰�, �� ����
    
    [Header("BattleObj : Battle Data")]
    // ü��, ��, �ֹ�������
    public int maxHP;
    public int curHP;
    public int Armor;
    public SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�
    public int strength = 0;
    public int intelligence = 0;
    
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    // �����̻� ����                                                      // <<<<<<<<<<<<<<<<<<<�ǵ��ߴ�
    private StatusEffectArray _StatusEffectArray = new StatusEffectArray(0);
    public Dictionary<StatusEffectType, int> StatusEffectDict = new Dictionary<StatusEffectType, int>(5);


    /// <summary>
    /// ���� ���� �����ؼ� ��Ÿ ���� ȿ���� ���� ���Ŀ� �ݿ����� �ʾ��� ���� �ʿ�
    /// </summary>
    /// <param name="_Object"></param>
    /// <param name="_DamageType"></param>
    /// <param name="_DamageValue"></param>

    #region BATLLEOBJ METHODS
    public void InitProfile(UnitInfo _UnitInfo)           // �ʱ�ȭ
    {
        maxHP = _UnitInfo.HP;
        curHP = _UnitInfo.HP;
        Armor = _UnitInfo.Armor;
        _SpellAdaptability = _UnitInfo._SpellAdaptability;
        strength = _UnitInfo.strength;
        intelligence = _UnitInfo.intelligence;
    }

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
#endregion
}

/*
public class Player : BattleObj
{
    // �÷��̾�� �Ϲ� ���� ��ü�ʹ� �޸� ī�� ���� ������ �Ӽ��� �޼��尡 �ʿ�
    public int Energy;              // ī�� ��� �ڽ�Ʈ ������
    public int Composure;           // ī�� �߰� ��ο� �ɷ�ġ�� ħ����

    public void Draw()              // ī�� �� �� ��ο�
    {
        
    }

}

*/