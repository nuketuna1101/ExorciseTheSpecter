using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Battle Scene�� ���� Battle Object ������ �ϴ� ������ ������Ʈ�� ���� ��ũ��Ʈ
/// </summary>


public class Unit : MonoBehaviour
{
    [SerializeField]
    private BattleObj _BattleObj;

    [Header("UnitPrefab Setup Sprite and TMP")]
    [SerializeField] private TMP_Text text_HP;
    [SerializeField] private TMP_Text text_Armor;
    [SerializeField] private TMP_Text text_SpellAdapt;
    [SerializeField] private TMP_Text text_Strength;
    [SerializeField] private TMP_Text text_Intelligence;
    [SerializeField] private TMP_Text text_Composure;
    [SerializeField] private TMP_Text text_Energy;


    //
    public void InitUnit(BattleObj _BattleObj)           // ���� ��ü ������ ����ֱ�
    {
        this._BattleObj = _BattleObj;
        RefreshVisual();
    }

    public void RefreshVisual()                           // ���� ������ �ؽ�Ʈ ������Ʈ
    {
        text_HP.text = _BattleObj.curHP + " / " + _BattleObj.maxHp;
        text_Armor.text = _BattleObj.Armor.ToString();
        text_Strength.text = _BattleObj.strength.ToString();
        text_Intelligence.text = _BattleObj.intelligence.ToString();
    }


}
