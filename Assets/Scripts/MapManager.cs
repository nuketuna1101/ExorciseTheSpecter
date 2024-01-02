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


    private void InitEdge()
    {
        // 현재 스테이지 넘버 가져오기.
        StageNumber = GameManager.Instance.StageNumber;
        int numberofstageChambers= 25;
        // 스테이지 넘버에 해당하는 데이터에 따라 맵 세팅

        // 데이터 테이블로부터 사전 세팅
        adj = new List<int>[numberofstageChambers + 1];
        visited = new bool[numberofstageChambers + 1];
        Array.Fill(visited, false);

        //


    }


}
