using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUnit : Unit
{
    private PlayerGameDataSO playerGameDataSO;
    [Header("Prefab texts control + Additional")]
    [SerializeField] private TMP_Text text_Composure;
    public int composure;

    #region 정보 초기화, 텍스트 랜더링
    public void InitPlayerUnit(PlayerGameDataSO playerGameDataSO)           // 초기화
    {
        this.playerGameDataSO = playerGameDataSO;
        unitInfo = playerGameDataSO.unitInfo;
    }
    public override void RefreshTexts()                           // 유닛 프리팹 텍스트 업데이트
    {
        base.RefreshTexts();
        text_Composure.text = this.composure.ToString();
    }
    #endregion

}
