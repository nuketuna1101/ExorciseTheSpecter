using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleObj
{
    // ���� ��ü�� ������ �� �޼ҵ�
    // �ൿ, ����, �����
    // �޼ҵ� �߻� ����? : �� ����, �� �߰�, �� ����

    [Header("BattleObj : Battle Data")]
    // ü��, ��, �ֹ�������
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�
    private int strength = 0;
    private int intelligence = 0;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    // �����̻� ����
    private StatusEffectArray _StatusEffectArray = new StatusEffectArray(0);




    public void Attack(BattleObj _Object, DamageType _DamageType, int _DamageValue)
    {
        // ��뿡�� ������ �� ���. ���� ������ ���� ���.
        if (_DamageType == DamageType.Physical)
        {
            // �������� ���� : ������ + strength��� �߰� ������
            // ���� ������ ���� ���� ������.
            int CalculatedDamageValue = _DamageValue + this.strength;
            if (this.Armor > 0) CalculatedDamageValue /= 2;
            //
            _Object.GetArmorReduced(CalculatedDamageValue);
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);
        }
        else if (_DamageType == DamageType.Magical)
        {
            // �������� ���� : ������ + intelligence ��� �߰� ������
            // �ֹ��������� ���� ����
            int CalculatedDamageValue = _DamageValue + this.intelligence;
            switch (_SpellAdaptability) 
            {
                case SpellAdaptability.None:
                    break;
                case SpellAdaptability.SpellResist:
                    CalculatedDamageValue -= (CalculatedDamageValue / 4);
                    break;
                case SpellAdaptability.SpellImmune:
                    CalculatedDamageValue = 0;
                    break;
                default:
                    break;
            }
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);
        }
        else if (_DamageType == DamageType.TrueDamage)
        {
            // ���� ������. ��� ��ġ �����ϰ� ��ġ �״�� ü�� ����
            _Object.BeAttacked(_DamageType, _DamageValue);
        }
        else
        {
            // ����
            DebugOpt.LogWarning("ERROR :: DamageType Exception");
        }
    }
    private void BeAttacked(DamageType _DamageType, int CalculatedDamageValue)
    {
        // ���� ��������ŭ �ǰ�
        this.curHP -= CalculatedDamageValue;

        // ���� ü�� 0 ���ϸ� ���ó��
    }
    public void GetArmorReduced(int value)
    {
        // �� ���� ����
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

}

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
    private int maxHp;
    private int curHP;
    private int Armor;
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