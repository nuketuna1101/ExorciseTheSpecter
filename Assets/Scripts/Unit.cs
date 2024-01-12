using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Battle Scene에 실제 Battle Object 역할을 하는 프리팹 오브젝트에 붙일 스크립트
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
    public void InitUnit(BattleObj _BattleObj)           // 전투 객체 데이터 집어넣기
    {
        this._BattleObj = _BattleObj;
        RefreshVisual();
    }

    public void RefreshVisual()                           // 유닛 프리팹 텍스트 업데이트
    {
        text_HP.text = _BattleObj.curHP + " / " + _BattleObj.maxHp;
        text_Armor.text = _BattleObj.Armor.ToString();
        text_Strength.text = _BattleObj.strength.ToString();
        text_Intelligence.text = _BattleObj.intelligence.ToString();
    }


}
