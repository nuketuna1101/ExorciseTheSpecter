using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주문 적응도: 
public enum SpellAdaptability { None, SpellResist, SpellImmune }
// 상태이상: 출혈, 중독, 탈진, 현기증, 피해망상, 속박
public enum StatusEffect { Bleed, Posion, Exhausted, Dizzy, Paranoia, Bind }
// 태세: 일반, 은신, 반격
public enum Stance { Normal, Stealth, CounterAtk }
// 피해 타입: 물리, 마법, 고정
public enum DamageType { Physical, Magical, TrueDamage }

public enum BuffType { Solid, Overwhelm, Intelligent, Composure }



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
