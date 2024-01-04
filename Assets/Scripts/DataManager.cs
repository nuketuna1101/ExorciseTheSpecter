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



    private void Awake()
    {
        ResetChamberInfo();
        SetChamberInfo(stageChamberNumber);

        InitEdge();

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
    }

    //-------------------------------------------------------------------------
    // ���� �÷��̾ ��ġ�� è�� ��ȣ (0: ó�� �������� ���� ����.)
    private int curChamberNumber = 0;

    private List<int> possibleChamberList;

    private void SetPossibleChamber()
    {
        // ���� ���¿��� ���� ������ è�� ����ȭ
        // �ʱ�ȭ
        possibleChamberList.Clear();
        possibleChamberList = adj[curChamberNumber].ToList();
    }

    private void EnterChamber(int _chamberNumber)
    {
        // �ѹ��� �ش��ϴ� è���� ������ ���,

        curChamberNumber = _chamberNumber;
    }

}
