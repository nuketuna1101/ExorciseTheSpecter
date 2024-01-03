using System;
using System.Collections;
using System.Collections.Generic;
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
        string file = "MapChamberInfo";
        dataList = CSVReader.Read(file);

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

    }



    private void InitEdge()
    {
        // �������� �ѹ��� �ش��ϴ� �����Ϳ� ���� �� ����

        // ������ ���̺�κ��� ���� ����
        // ���� ���� ����Ʈ adj ����
        adj = new List<int>[stageChamberNumber + 1];
        for (int i = 1; i <= stageChamberNumber; i++)
        {
            adj[i] = new List<int>();
            if (_StageChamberSO.StageChamberArray[i].NextChamber1 != -1)
                adj[i].Add(_StageChamberSO.StageChamberArray[i].NextChamber1);
            if (_StageChamberSO.StageChamberArray[i].NextChamber2 != -1)
                adj[i].Add(_StageChamberSO.StageChamberArray[i].NextChamber2);
        }
        // �湮 �迭 visited ����
        visited = new bool[stageChamberNumber + 1];
        Array.Fill(visited, false);
    }
}
