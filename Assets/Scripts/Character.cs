using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleObj
{
    // 전투 객체가 가져야 할 메소드
    // 행동, 버프, 디버프
    // 메소드 발생 시점? : 턴 시작, 턴 중간, 턴 종료

    [Header("BattleObj : Battle Data")]
    // 체력, 방어도, 주문적응력
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // 강력함, 총명
    private int strength = 0;
    private int intelligence = 0;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    // 상태이상 지속
    private StatusEffectArray _StatusEffectArray = new StatusEffectArray(0);




    public void Attack(BattleObj _Object, DamageType _DamageType, int _DamageValue)
    {
        // 상대에게 공격을 할 경우. 피해 종류에 따라 계산.
        if (_DamageType == DamageType.Physical)
        {
            // 물리피해 계산식 : 데미지 + strength기반 추가 데미지
            // 방어도가 있으면 절반 깎은 데미지.
            int CalculatedDamageValue = _DamageValue + this.strength;
            if (this.Armor > 0) CalculatedDamageValue /= 2;
            //
            _Object.GetArmorReduced(CalculatedDamageValue);
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);
        }
        else if (_DamageType == DamageType.Magical)
        {
            // 마법피해 계산식 : 데미지 + intelligence 기반 추가 데미지
            // 주문적응도에 따라 차등
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
            // 고정 데미지. 모든 수치 무시하고 수치 그대로 체력 적용
            _Object.BeAttacked(_DamageType, _DamageValue);
        }
        else
        {
            // 에러
            DebugOpt.LogWarning("ERROR :: DamageType Exception");
        }
    }
    private void BeAttacked(DamageType _DamageType, int CalculatedDamageValue)
    {
        // 계산된 데미지만큼 피격
        this.curHP -= CalculatedDamageValue;

        // 최종 체력 0 이하면 사망처리
    }
    public void GetArmorReduced(int value)
    {
        // 방어도 깎임 적용
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

}

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
    private int maxHp;
    private int curHP;
    private int Armor;
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