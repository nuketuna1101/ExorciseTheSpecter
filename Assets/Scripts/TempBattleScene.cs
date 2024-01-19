using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ʋ�� ���� �κ� �ڵ忡 ���ؼ� �ӽ� �׽�Ʈ �ڵ� ���� ���
/// </summary>


public class TempBattleScene : MonoBehaviour
{

    public void TestTempBattleInit()
    {
        //BattleManager.Instance.TestMethod();

        // ������ ����
        BattleManager.Instance.SetPlayer();
        BattleManager.Instance.SetEnemy();

        // �� 
        BattleManager.Instance.InitTurnSystem();

        // �� �ʱ�ȭ
        CardManager.Instance.TestInitDeck();

        // ���� ī�� �̱�
        CardManager.Instance.DrawCards(4);
    }

}
