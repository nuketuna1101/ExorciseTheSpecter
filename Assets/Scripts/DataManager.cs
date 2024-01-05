using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static StageChamberSO;

public class DataManager : Singleton<DataManager>
{
    //
    private string[] fileNames = { "MapChamberInfo" };


    [SerializeField]
    private StageChamberSO _StageChamberSO;
    private int stageChamberNumber = 13;

    // 인접리스트와 방문노드
    [SerializeField]
    public List<int>[] adj;
    [SerializeField]
    private bool[] visited;

    private ChamberState[] _ChamberStates;      public ChamberState[] publicChamberStates { get { return _ChamberStates; } }



    private void Awake()
    {
        ResetChamberInfo();
        SetChamberInfo(stageChamberNumber);

        InitEdge();

    }

    // StageChamberSO 초기화
    private void ResetChamberInfo()
    {      
        _StageChamberSO.StageChamberArray.Clear();
    }
    // CSV로부터 StageChamberSO 저장
    private void SetChamberInfo(int _StageChamberNumber)
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "ChamberInfo";
        dataList = CSVReader.Read(file);
        /*
        _StageChamberSO.StageChamberArray.Add( new ChamberArray
        {
            StageNumber = 0,
            ChamberNumber = 0,
            ChamberType = 0,
            NextChamber1 = 0,
            NextChamber2 = 0,
        }
        );

        for (int i = 0; i < _StageChamberNumber; i++)
        {
            _StageChamberSO.StageChamberArray.Add( new ChamberArray
            {
                StageNumber = CSVReader.GetIntValue(dataList, i, "StageNumber"),
                ChamberNumber = CSVReader.GetIntValue(dataList, i, "ChamberNumber"),
                ChamberType = CSVReader.GetIntValue(dataList, i, "ChamberType"),
                NextChamber1 = CSVReader.GetIntValue(dataList, i, "NextChamber1"),
                NextChamber2 = CSVReader.GetIntValue(dataList, i, "NextChamber2"),
            }
            );
        }
        */
        for (int i = 0; i <= _StageChamberNumber; i++)
        {
            _StageChamberSO.StageChamberArray.Add(new ChamberArray
            {
                StageNumber = CSVReader.GetIntValue(dataList, i, "StageNumber"),
                ChamberNumber = CSVReader.GetIntValue(dataList, i, "ChamberNumber"),
                ChamberType = CSVReader.GetIntValue(dataList, i, "ChamberType"),
                NextChamber1 = CSVReader.GetIntValue(dataList, i, "NextChamber1"),
                NextChamber2 = CSVReader.GetIntValue(dataList, i, "NextChamber2"),
                NextChamber3 = CSVReader.GetIntValue(dataList, i, "NextChamber3"),
            }
            );
        }
    }



    private void InitEdge()
    {
        // 스테이지 넘버에 해당하는 데이터에 따라 맵 세팅

        // 데이터 테이블로부터 사전 세팅
        // 인접 엣지 리스트 adj 세팅
        adj = new List<int>[stageChamberNumber + 1];
        for (int i = 0; i <= stageChamberNumber; i++)
        {
            adj[i] = new List<int>();
            var curStageChamber = _StageChamberSO.StageChamberArray[i];
            if (curStageChamber.NextChamber1 != -1)
                adj[i].Add(curStageChamber.NextChamber1);
            if (curStageChamber.NextChamber2 != -1)
                adj[i].Add(curStageChamber.NextChamber2);
            if (curStageChamber.NextChamber3 != -1)
                adj[i].Add(curStageChamber.NextChamber3);
        }
        // 방문 배열 visited 세팅
        visited = new bool[stageChamberNumber + 1];
        Array.Fill(visited, false);
        visited[0] = true;
        // 챔버 상태 배열 초기화
        _ChamberStates = new ChamberState[stageChamberNumber + 1];


        //
        UpdateChamberStates();
    }

    //-------------------------------------------------------------------------
    // 현재 플레이어가 위치한 챔버 번호 (0: 처음 스테이지 최초 진입.)

    // 현재 상태의 챔버 상태 업데이트
    private void UpdateChamberStates()
    {
        // 기본 rest of으로 초기화
        Array.Fill(_ChamberStates, ChamberState.RestOf);
        // 방문한 챔버 업데이트
        for (int i = 1; i <= stageChamberNumber; i++)
        {
            if (visited[i])
                _ChamberStates[i] = ChamberState.Visited;
        }
        // 현재 상태 기반 방문 가능 업데이트
        foreach (var chamber in adj[GameManager.Instance.LastCompletedChamberNumber])
        {
            _ChamberStates[chamber] = ChamberState.Accessable;
        }
        // 선택된 거 업데이트
        _ChamberStates[GameManager.Instance.CurSelectedChamberNumber] = ChamberState.Selected;
    }
}
