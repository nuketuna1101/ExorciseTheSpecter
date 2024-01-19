using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// ui manager로 추후 리팩토링 될 부분!!!!!!!!!!!!!!!!!!!!
/// </summary>


public class TopBarUIManager : MonoBehaviour
{
    /// <summary>
    /// TopBarUIManager ::
    /// 
    /// </summary>
    private readonly string[] charNames = { "ROGUE", "GUNSLINGER" };
    private TMP_Text _Text_CharName;
    private TMP_Text _Text_HP;
    private TMP_Text _Text_Gold;
    private TMP_Text _Text_StageNumber;
    private PlayerGameDataSO _PlayerGameDataSO;

    // Data view update
    private void Awake()
    {
        // 텍스트 매칭
        _Text_CharName =    this.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        _Text_HP =          this.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _Text_Gold =        this.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _Text_StageNumber = this.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        _PlayerGameDataSO = DataManager.Instance._PlayerGameDataSO;
        UpdateValue();
    }
    private void UpdateValue()
    {
        _Text_CharName.text =    charNames[_PlayerGameDataSO.characterCode];
        _Text_HP.text =          String.Format("{0} / {1}", _PlayerGameDataSO.curHP, _PlayerGameDataSO.maxHp);
        _Text_Gold.text = "" + _PlayerGameDataSO.gold;
        _Text_StageNumber.text = "" + _PlayerGameDataSO.currentStageNumber;
    }
}
