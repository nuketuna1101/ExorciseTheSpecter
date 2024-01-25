using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyWiki enemyWiki;
    [Header("Prefab texts control + Additional")]
    [SerializeField] private TMP_Text text_Name;
    private int battleManagerenemyListIndex;                    // 배틀 매니저에서 존재중인 에너미 리스트 안에서 차지하는 인덱스 

    #region 정보 초기화, 텍스트 랜더링
    public void InitEnemyUnit(EnemyWiki _EnemyWiki, int _battleManagerenemyListIndex)           // 초기화
    {
        enemyWiki = _EnemyWiki;
        unitInfo = _EnemyWiki.unitInfo;
        battleManagerenemyListIndex = _battleManagerenemyListIndex;
    }
    public override void RefreshTexts()                           // 유닛 프리팹 텍스트 업데이트
    {
        base.RefreshTexts();
        text_Name.text = enemyWiki.EnemyName;
    }
    public int GetBattleManagerEnemyListIndex()
    {
        return battleManagerenemyListIndex;
    }
    #endregion

}
