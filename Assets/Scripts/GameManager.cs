using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : Singleton<GameManager>
{
    [Header("TopBar GameData")]
    private int characterCode = -1;                 public int CharacterCode { get { return characterCode; } set { characterCode = value; } }
    private int curHP = 25;                          public int CurHP { get { return curHP; } set { curHP = value; } }
    private int maxHP = 100;                          public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    private int gold = 250;                           public int Gold { get { return gold; } set { gold = value; } }
    private int stageNumber = 1;                    public int StageNumber { get { return stageNumber; } set { stageNumber = value; } }

    [Header("InBattle Player Stats")]
    private int Energy = 3;
    private int Armor;
    private int Strength;


    // 여기에 있을지 모르겟지만 일단 임시로
    private int lastCompletedChamberNumber = 0;          public int LastCompletedChamberNumber { get { return lastCompletedChamberNumber; } set { lastCompletedChamberNumber = value; } }
    private int curSelectedChamberNumber = -1;           public int CurSelectedChamberNumber { get { return curSelectedChamberNumber; } set { curSelectedChamberNumber = value; } }
    private int curEnteredChamberNumber = -1;            public int CurEnteredChamberNumber { get { return curEnteredChamberNumber; } set { curEnteredChamberNumber = value; } }

    private List<int> accessableChamberList;

    [Header("Player Deck")]
    public List<CardInfo> PlayerDeck;




    //-------------------------------------------
    /// 배틀 씬 상황이라고 가정을 해보자. 이 코드를 게임매니저가 가지고 있을진 모르겠지만 일단 구현
    /// 
    //------------------------------------
    
    // 배틀 씬 입장하면, 우선 플레이어와 적 유닛을 초기화 세팅

    public void InitEnterBattle()
    {
        // (1) 기본 세팅
        // 플레이어 프리팹 생성, 플레이어 데이터 로딩해와서 바인딩
        // enemy도 마찬가지 작업
        // 카드 관련: 플레이어의 보유 덱 로딩

        // (2) 턴: 플레이어 턴으로 시작
        // 턴 시작 시 사용 에너지 할당.
        // 기본 드로우만큼 카드 핸드로 드로우


        // 턴 종료 버튼 눌러서 상대에게 턴 넘기기.
        



    }


    // 일단 임시로 에너지 소모관련
    public int GetEnergy()
    {
        return Energy;
    }

    public void ConsumeEnergy(int value)
    {
        Energy -= value;
    }







    private bool isSelectedAny = false;
    private int selectedCode = -1;
    private IEnumerator FlashingText()
    {
        int i = 0;
        while (true)
        {
            // 오직 아무것도 선택 안되어 있을 때만.
            if (isSelectedAny) break;
            //
            yield return new WaitForSeconds(0.1f);
            //_Text.color = colorArr[i];
            //i = (i + 1) % colorArr.Length;
        }
    }
    public void ShowSelectedCharacter(int _characterCode)                   // character select 버튼의 로직
    {
        if (!isSelectedAny)        // 최초 실행 시, 선택된 캐릭터 없다가 선택된 캐릭터가 생김
        {
            isSelectedAny = true;
            //UIManager.Instance.Scene1_TEMP();       // 캐릭터 정보 팝업창, 출정 버튼 활성화
        }

        // 이미 선택된 것이면 아무런 행동x
        if (selectedCode == characterCode)
            return;

        // 변경 시 선택된 캐릭터 코드 수정
        selectedCode = _characterCode;
        characterCode = selectedCode;
        // 정보창 내용 및 이미지 수정
        //UIManager.Instance.Scene1_TEMP2();       // 캐릭터 정보 팝업창, 출정 버튼 활성화
    }

}
