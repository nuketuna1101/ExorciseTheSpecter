using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField]
    private Player _Player;

    [Header("PlayerUnitPrefab : texts")]
    [SerializeField] private TMP_Text text_HP;
    [SerializeField] private TMP_Text text_Armor;
    [SerializeField] private TMP_Text text_SpellAdapt;
    [SerializeField] private TMP_Text text_Strength;
    [SerializeField] private TMP_Text text_Intelligence;
    [SerializeField] private TMP_Text text_Composure;
    [SerializeField] private TMP_Text text_Energy;


    //
    public void InitUnit(Player _Player)           // 전투 객체 데이터 집어넣기
    {
        this._Player = _Player;
        RefreshTexts();
    }

    public void RefreshTexts()                           // 유닛 프리팹 텍스트 업데이트
    {
        text_HP.text = _Player.curHP + "/" + _Player.maxHp;
        text_Armor.text = _Player.Armor.ToString();
        text_SpellAdapt.text = _Player._SpellAdaptability.ToString();
        text_Strength.text = _Player.strength.ToString();
        text_Intelligence.text = _Player.intelligence.ToString();
        //text_Composure.text = _Player.composure.ToString();
        //text_Energy.text = _Player.energy.ToString();
    }
}
