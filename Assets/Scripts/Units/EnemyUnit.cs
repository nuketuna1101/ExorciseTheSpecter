using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyWiki enemyWiki;
    [Header("Prefab texts control + Additional")]
    [SerializeField] private TMP_Text text_Name;

    #region 정보 초기화, 텍스트 랜더링
    public void InitEnemyUnit(EnemyWiki _EnemyWiki)           // 초기화
    {
        enemyWiki = _EnemyWiki;
        unitInfo = _EnemyWiki.unitInfo;
    }
    public override void RefreshTexts()                           // 유닛 프리팹 텍스트 업데이트
    {
        base.RefreshTexts();
        text_Name.text = enemyWiki.EnemyName;
    }
    #endregion

}
