using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ֹ� ������: 
public enum SpellAdaptability { None, Resist, Immune }
// �����̻�: ����, �ߵ�, Ż��, ������, ���ظ���, �ӹ�
public enum StatusEffectType { Bleeding = 1, Posioned, Exhausted, Dizzy, Paranoia }
// �¼�: �Ϲ�, ����, �ݰ�
public enum Stance { Normal, Stealth, CounterAtk }
// ���� Ÿ��: ����, ����, ����
public enum DamageType { Physical = 1, Magical, TrueDamage }

public enum BuffType { Solid, Overwhelm, Intelligent, Composure }
// 0: �߰� / 1: �� / 2: �Ѹ� / 3: ħ��/ 4: ī�� ��ο�/ 5: ü�� ȸ��



public struct StatusEffect
{
    public StatusEffectType statusEffectType;
    public int duration;

    public StatusEffect(StatusEffectType statusEffectType, int duration)
    {
        this.statusEffectType = statusEffectType;
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

    public void AddValue(StatusEffect statusEffect)
    {
        int index = (int)statusEffect.statusEffectType - 1;
        this.SEarray[index].duration += statusEffect.duration;
        /*
        switch (statusEffect._StatusEffectType)
        {
            case StatusEffectType.Bleeding:
                this.SEarray[0].duration += statusEffect.duration;
                break;
            case StatusEffectType.Posioned:
                this.SEarray[1].duration += statusEffect.duration;
                break;
            case StatusEffectType.Exhausted:
                this.SEarray[2].duration += statusEffect.duration;
                break;
            case StatusEffectType.Dizzy:
                this.SEarray[3].duration += statusEffect.duration;
                break;
            case StatusEffectType.Paranoia:
                this.SEarray[4].duration += statusEffect.duration;
                break;
        }
        */
    }
    public void SetZero(StatusEffectType statusEffectType)
    {
        int index = (int)statusEffectType - 1;
        this.SEarray[index].duration = 0;
        /*
        switch (statusEffectType)
        {
            case StatusEffectType.Bleeding:
                this.SEarray[0].duration = 0;
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
        */
    }
}


public class KeywordDictionary
{

}
