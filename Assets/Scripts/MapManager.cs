using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    // 맵 매니저 : 에어리어 선택

    //
    private bool isSelectedAny = false;
    private int selectedCode = -1;

    // 에어리어의 상태 : 이미 방문함, 현재 방문가능함, 현재 방문 불가능
    private int StageNumber;
    private int totalChamberNumber;
    // 인접리스트와 방문노드
    private List<int>[] adj;
    private bool[] visited;

    // 현재 플레이어가 위치한 챔버 번호 (0: 처음 스테이지 최초 진입.)
    private int curChamberNumber = 0;

    private List<int> possibleChamberList;

    private void SetPossibleChamber()
    {
        // 현재 상태에서 진입 가능한 챔버 가시화
        // 초기화
        possibleChamberList.Clear();

    }


}
