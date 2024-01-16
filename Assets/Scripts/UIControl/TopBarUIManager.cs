using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    // Data view update
    private void Awake()
    {
        // ÅØ½ºÆ® ¸ÅÄª
        _Text_CharName =    this.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        _Text_HP =          this.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _Text_Gold =        this.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _Text_StageNumber = this.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        UpdateValue();
    }
    private void UpdateValue()
    {
        _Text_CharName.text =    charNames[GameManager.Instance.CharacterCode];
        _Text_HP.text =          String.Format("{0} / {1}", GameManager.Instance.CurHP, GameManager.Instance.MaxHP);
        _Text_Gold.text = "" + GameManager.Instance.Gold;//String.Format("{0}", GameManager.Instance.CurHP, GameManager.Instance.MaxHP);
        _Text_StageNumber.text = "" + GameManager.Instance.StageNumber;//String.Format("{0} / {1}", GameManager.Instance.CurHP, GameManager.Instance.MaxHP);
    }
}
