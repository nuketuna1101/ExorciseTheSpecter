using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public abstract class Unit : MonoBehaviour              // 추상클래스 -> PlayerUnit, EnemyUnit을 구현.
{
    [Header("Unit : Battle Data")]
    [SerializeField] protected UnitInfo unitInfo;       // 유닛 클래스에 적용될 유닛 정의 데이터
    public int maxHP;                                   //
    public int curHP;                                   //
    public int Armor;                                   //
    public SpellAdaptability _SpellAdaptability;        //
    public int strength;                                //
    public int intelligence;                            //
    protected Stance _Stance = Stance.Normal;             // 태세: 일반, 은신, 반격
    // 상태이상 지속                                                      // <<<<<<<<<<<<<<<<<<<건들어야댐
    public StatusEffectArray statusEffectArray = new StatusEffectArray(0);
    public Dictionary<StatusEffectType, int> StatusEffectDict = new Dictionary<StatusEffectType, int>(5);


    [Header("Prefab texts control")]
    [SerializeField] protected TMP_Text text_HP;
    [SerializeField] protected TMP_Text text_Armor;
    [SerializeField] protected TMP_Text text_SpellAdapt;
    [SerializeField] protected TMP_Text text_Strength;
    [SerializeField] protected TMP_Text text_Intelligence;

    #region 정보 초기화, 텍스트 랜더링
    public void InitUnitProperty()                      // unitInfo로부터 배틀 데이터 속성 초기화
    {
        maxHP = unitInfo.HP;
        curHP = unitInfo.HP;
        Armor = unitInfo.Armor;
        _SpellAdaptability = unitInfo._SpellAdaptability;
        strength = unitInfo.strength;
        intelligence = unitInfo.intelligence;
    }
    public virtual void RefreshTexts()
    {
        text_HP.text = this.curHP + "/" + this.maxHP;
        text_Armor.text = this.Armor.ToString();
        text_SpellAdapt.text = this._SpellAdaptability.ToString();
        text_Strength.text = this.strength.ToString();
        text_Intelligence.text = this.intelligence.ToString();
    }
    #endregion
}