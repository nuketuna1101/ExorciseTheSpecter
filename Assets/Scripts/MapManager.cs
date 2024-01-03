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



}
