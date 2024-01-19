using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배틀씬 최초 부분 코드에 대해서 임시 테스트 코드 구현 장소
/// </summary>


public class TempBattleScene : MonoBehaviour
{

    public void TestTempBattleInit()
    {
        //BattleManager.Instance.TestMethod();

        // 데이터 세팅
        BattleManager.Instance.SetPlayer();
        BattleManager.Instance.SetEnemy();

        // 턴 
        BattleManager.Instance.InitTurnSystem();

        // 덱 초기화
        CardManager.Instance.TestInitDeck();

        // 시작 카드 뽑기
        CardManager.Instance.DrawCards(4);
    }

}
