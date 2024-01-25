using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyWiki enemyWiki;
    [Header("Prefab texts control + Additional")]
    [SerializeField] private TMP_Text text_Name;
    private int battleManagerenemyListIndex;                    // ��Ʋ �Ŵ������� �������� ���ʹ� ����Ʈ �ȿ��� �����ϴ� �ε��� 

    #region ���� �ʱ�ȭ, �ؽ�Ʈ ������
    public void InitEnemyUnit(EnemyWiki _EnemyWiki, int _battleManagerenemyListIndex)           // �ʱ�ȭ
    {
        enemyWiki = _EnemyWiki;
        unitInfo = _EnemyWiki.unitInfo;
        battleManagerenemyListIndex = _battleManagerenemyListIndex;
    }
    public override void RefreshTexts()                           // ���� ������ �ؽ�Ʈ ������Ʈ
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
