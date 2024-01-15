using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle Scene에서 전투 상호작용에 필요한 객체 클래스와 메소드 구현
/// </summary>

public abstract class BattleObj
{
    // 전투 객체가 가져야 할 메소드
    // 행동, 버프, 디버프
    // 메소드 발생 시점? : 턴 시작, 턴 중간, 턴 종료
    [Header("BattleObj : Battle Data")]
    // 체력, 방어도, 주문적응력
    public int maxHp = 400;
    public int curHP = 400;
    public int Armor = 16;
    public SpellAdaptability _SpellAdaptability;
    // 강력함, 총명
    public int strength = 0;
    public int intelligence = 0;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    // 상태이상 지속
    private StatusEffectArray _StatusEffectArray = new StatusEffectArray(0);
    public Dictionary<StatusEffectType, int> StatusEffectDict = new Dictionary<StatusEffectType, int>(5);


    /// <summary>
    /// 공격 피해 관련해서 기타 절감 효과를 아직 계산식에 반영하지 않았음 수정 필요
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
            // 힘만큼 추가 데미지, 상태이상'Exhausted' 시 데미지 경감
            CalculatedDamageValue += this.strength;

        }
        else if (_DamageType == DamageType.Magical)
        {
            // 지능만큼 추가 데미지, 상태이상'Dizzy' 시 데미지 경감
            CalculatedDamageValue += this.intelligence;
        }
        _Object.BeAttacked(_DamageType, CalculatedDamageValue);
    }
    private void BeAttacked(DamageType _DamageType, int CalculatedDamageValue)
    {
        // 계산된 데미지만큼 피격
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


        // 최종 체력 0 이하면 사망처리
    }
    public void GetArmorReduced(int value)
    {
        // 방어도 깎임 적용
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        this.Armor = (this.Armor >= value ? this.Armor - value : 0);
    }
    public void GiveStatusEffect(BattleObj _Object, StatusEffect _StatusEffect)
    {
        // 상대에게 상태 이상 효과를 지속 턴만큼 부여
        _Object.GetStatusEffect(_StatusEffect);
    }
    private void GetStatusEffect(StatusEffect _StatusEffect)
    {
        // 상태이상 효과 적용
        // 주의: 수치나 적용에 대한 갱신일뿐 실제 효과 적용은 나중에
        _StatusEffectArray.AddValue(_StatusEffect);
    }

    public void GetEffectWhenTurnStarts()
    {
        // 턴 시작 시 받는 효과 발동
        // 효과 큐에 넣어서 실행




    }
    public void GetEffectWhenTurnEnds()
    {
        // 턴 종료 시 받는 효과 발동

        // 출혈, 중독은 턴 종료 시 발동됨
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
    // 체력, 방어도, 주문적응력
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // 강력함, 총명, 침착
    private int strength;
    private int intelligence;
    private int composure;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    // 축복, 저주
    private List<int> Blessings;
    private int Curses;
}

public class Enemy : BattleObj
{
    [Header("Enemy: Indentity")]
    private int EnemyID;

    [Header("Enemy : Battle Data")]
    // 체력, 방어도, 주문적응력
    private int maxHp = 100;
    private int curHP = 100;
    private int Armor = 25;
    private SpellAdaptability _SpellAdaptability;
    // 강력함, 총명
    private int strength = 0;
    private int intelligence = 0;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    //
    private List<int> ActionList;
}



public class BattleJudge
{
    /// <summary>
    /// 전투 판정 관련
    /// </summary>
    //

    public void HitDmg(BattleObj _Subject, BattleObj _Object, int _DmgAmount, int _DmgType)
    {

    }

}
*/