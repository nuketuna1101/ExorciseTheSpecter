using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyWiki enemyWiki;
    [Header("Prefab texts control + Additional")]
    [SerializeField] private TMP_Text text_Name;

    #region ���� �ʱ�ȭ, �ؽ�Ʈ ������
    public void InitEnemyUnit(EnemyWiki _EnemyWiki)           // �ʱ�ȭ
    {
        enemyWiki = _EnemyWiki;
        unitInfo = _EnemyWiki.unitInfo;
    }
    public override void RefreshTexts()                           // ���� ������ �ؽ�Ʈ ������Ʈ
    {
        base.RefreshTexts();
        text_Name.text = enemyWiki.EnemyName;
    }
    #endregion

}
