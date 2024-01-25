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

    #region ���� �ʱ�ȭ, �ؽ�Ʈ ������
    public void InitPlayerUnit(PlayerGameDataSO playerGameDataSO)           // �ʱ�ȭ
    {
        this.playerGameDataSO = playerGameDataSO;
        unitInfo = playerGameDataSO.unitInfo;
    }
    public override void RefreshTexts()                           // ���� ������ �ؽ�Ʈ ������Ʈ
    {
        base.RefreshTexts();
        text_Composure.text = this.composure.ToString();
    }
    #endregion

}
