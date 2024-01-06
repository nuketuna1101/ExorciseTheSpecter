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

    // ��������Ʈ�� �湮���
    [SerializeField]
    public List<int>[] adj;
    [SerializeField]
    private bool[] visited;

    [SerializeField]
    private ChamberState[] _ChamberStates;      public ChamberState[] publicChamberStates { get { return _ChamberStates; } }



    private void Awake()
    {
        ResetChamberInfo();
        SetChamberInfo(stageChamberNumber);

        InitEdge();

        StartCoroutine(UpdateChamberStateCrtn());
    }

    // StageChamberSO �ʱ�ȭ
    private void ResetChamberInfo()
    {      
        _StageChamberSO.StageChamberArray.Clear();
    }
    // CSV�κ��� StageChamberSO ����
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
        // �������� �ѹ��� �ش��ϴ� �����Ϳ� ���� �� ����

        // ������ ���̺�κ��� ���� ����
        // ���� ���� ����Ʈ adj ����
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
        // �湮 �迭 visited ����
        visited = new bool[stageChamberNumber + 1];
        Array.Fill(visited, false);
        visited[0] = true;
        // è�� ���� �迭 �ʱ�ȭ
        _ChamberStates = new ChamberState[stageChamberNumber + 1];
        //
    }

    //-------------------------------------------------------------------------
    // ���� �÷��̾ ��ġ�� è�� ��ȣ (0: ó�� �������� ���� ����.)

    // ���� ������ è�� ���� ������Ʈ
    private void UpdateChamberStates()
    {
        // �⺻ rest of���� �ʱ�ȭ
        Array.Fill(_ChamberStates, ChamberState.RestOf);
        // �湮�� è�� ������Ʈ
        for (int i = 0; i <= stageChamberNumber; i++)
        {
            if (visited[i])
                _ChamberStates[i] = ChamberState.Visited;
        }

        // ���õ� �� ������Ʈ
        if (GameManager.Instance.CurSelectedChamberNumber != -1)
            _ChamberStates[GameManager.Instance.CurSelectedChamberNumber] = ChamberState.Selected;


        // ���� ���� ��� �湮 ���� ������Ʈ
        foreach (var chamberNumber in adj[GameManager.Instance.LastCompletedChamberNumber])
        {
            if (chamberNumber == GameManager.Instance.CurSelectedChamberNumber) 
                continue;
            _ChamberStates[chamberNumber] = ChamberState.Accessable;
        }

    }


    private IEnumerator UpdateChamberStateCrtn()
    {
        while (true)
        {
            yield return null;
            UpdateChamberStates();
        }
    }

}
