using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public abstract class Unit : MonoBehaviour              // �߻�Ŭ���� -> PlayerUnit, EnemyUnit�� ����.
{
    [Header("Unit : Battle Data")]
    [SerializeField] protected UnitInfo unitInfo;       // ���� Ŭ������ ����� ���� ���� ������
    public int maxHP;                                   //
    public int curHP;                                   //
    public int Armor;                                   //
    public SpellAdaptability _SpellAdaptability;        //
    public int strength;                                //
    public int intelligence;                            //
    protected Stance _Stance = Stance.Normal;             // �¼�: �Ϲ�, ����, �ݰ�
    // �����̻� ����                                                      // <<<<<<<<<<<<<<<<<<<�ǵ��ߴ�
    public StatusEffectArray statusEffectArray = new StatusEffectArray(0);
    public Dictionary<StatusEffectType, int> StatusEffectDict = new Dictionary<StatusEffectType, int>(5);


    [Header("Prefab texts control")]
    [SerializeField] protected TMP_Text text_HP;
    [SerializeField] protected TMP_Text text_Armor;
    [SerializeField] protected TMP_Text text_SpellAdapt;
    [SerializeField] protected TMP_Text text_Strength;
    [SerializeField] protected TMP_Text text_Intelligence;

    #region ���� �ʱ�ȭ, �ؽ�Ʈ ������
    public void InitUnitProperty()                      // unitInfo�κ��� ��Ʋ ������ �Ӽ� �ʱ�ȭ
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