using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주문 적응도: 
public enum SpellAdaptability { None, Resist, Immune }
// 상태이상: 출혈, 중독, 탈진, 현기증, 피해망상, 속박
public enum StatusEffectType { Bleeding, Posioned, Exhausted, Dizzy, Paranoia }
// 태세: 일반, 은신, 반격
public enum Stance { Normal, Stealth, CounterAtk }
// 피해 타입: 물리, 마법, 고정
public enum DamageType { Physical, Magical, TrueDamage }

public enum BuffType { Solid, Overwhelm, Intelligent, Composure }



public struct StatusEffect
{
    public StatusEffectType _StatusEffectType;
    public int duration;

    public StatusEffect(StatusEffectType _StatusEffectType, int duration)
    {
        this._StatusEffectType = _StatusEffectType;
        this.duration = duration;
    }
}

public struct StatusEffectArray
{
    public StatusEffect[] SEarray;

    public StatusEffectArray(int initValue)
    {
        SEarray = new StatusEffect[5];
        SEarray[0] = new StatusEffect(StatusEffectType.Bleeding, initValue);
        SEarray[1] = new StatusEffect(StatusEffectType.Posioned, initValue);
        SEarray[2] = new StatusEffect(StatusEffectType.Exhausted, initValue);
        SEarray[3] = new StatusEffect(StatusEffectType.Dizzy, initValue);
        SEarray[4] = new StatusEffect(StatusEffectType.Paranoia, initValue);
    }

    public void AddValue(StatusEffect _StatusEffect)
    {
        switch (_StatusEffect._StatusEffectType)
        {
            case StatusEffectType.Bleeding:
                this.SEarray[0].duration += _StatusEffect.duration;
                break;
            case StatusEffectType.Posioned:
                this.SEarray[1].duration += _StatusEffect.duration;
                break;
            case StatusEffectType.Exhausted:
                this.SEarray[2].duration += _StatusEffect.duration;
                break;
            case StatusEffectType.Dizzy:
                this.SEarray[3].duration += _StatusEffect.duration;
                break;
            case StatusEffectType.Paranoia:
                this.SEarray[4].duration += _StatusEffect.duration;
                break;
        }
    }
}









public class KeywordDictionary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
