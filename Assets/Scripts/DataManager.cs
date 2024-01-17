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
    /// <summary>
    /// DataManager ::
    /// convert data FROM CSV datatable TO Scriptable Object
    /// </summary>
    
    [Header("Chamber Data")]
    private readonly string[] fileNames = { "ChamberInfo" };
    [SerializeField] private StageChamberSO _StageChamberSO;
    // ��������Ʈ�� �湮���
    [SerializeField] public List<int>[] adj;
    [SerializeField] private bool[] visited;
    [SerializeField] private ChamberState[] _ChamberStates;      
    public ChamberState[] publicChamberStates { get { return _ChamberStates; } }
    private readonly int stageChamberNumber = 13;
    public int TotalChamberNumber;

    [Header("Card Data")]
    [SerializeField] private CardInfoSO _CardInfoSO;
    public CardInfoSO _TempAccessCardInfoSO { get { return _CardInfoSO; } }
    public int TotalCardNumber;

    [Header("Character Data")]
    [SerializeField] private Sprite[] CharacterClassSprites;     // �÷��̾� ���� Ŭ���� �⺻ �̹��� ��������Ʈ
    [SerializeField] private UnitInfoSO PlayerUnitInfoSO_Init;
    [SerializeField] private UnitInfoSO PlayerUnitInfoSO_Current;

    private new void Awake()
    {
        base.Awake();
        //
        ResetChamberInfo();
        SetChamberInfo();

        //
        ResetCardInfo();
        SetCardInfo();

        InitEdge();

        StartCoroutine(UpdateChamberStateCrtn());
    }

    #region Chamber ������ �ʱ�ȭ �ҷ����� ����
    private void ResetChamberInfo()    // StageChamberSO �ʱ�ȭ
    {
        _StageChamberSO.ChamberInfoList.Clear();
    }
    private void SetChamberInfo()    // CSV�κ��� StageChamberSO ����
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "ChamberInfo";
        dataList = CSVReader.Read(file);
        //
        TotalChamberNumber = CSVReader.GetLinesLength(file) - 2;
        for (int i = 0; i < TotalChamberNumber; i++)
        {
            _StageChamberSO.ChamberInfoList.Add(new ChamberInfo
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
    #endregion

    #region Card ����
    private void ResetCardInfo()
    {
        _CardInfoSO.CardInfoList.Clear();
    }
    private void SetCardInfo()
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "CardInfo";
        dataList = CSVReader.Read(file);
        //
        TotalCardNumber = CSVReader.GetLinesLength(file) - 2;
        for (int i = 0; i < TotalCardNumber; i++)
        {
            _CardInfoSO.CardInfoList.Add(new CardInfo
            {
                Class = CSVReader.GetIntValue(dataList, i, "Class"),
                CardID = CSVReader.GetIntValue(dataList, i, "CardID"),
                CardType = CSVReader.GetIntValue(dataList, i, "CardType"),
                CardCost = CSVReader.GetIntValue(dataList, i, "CardCost"),
                CardName = CSVReader.GetString(dataList, i, "CardName"),
                CardDescription = CSVReader.GetString(dataList, i, "CardContent"),
            }
            );
        }
    }
    #endregion

    #region Chamber�� ���� ����    // ���� �÷��̾ ��ġ�� è�� ��ȣ (0: ó�� �������� ���� ����.)
    private void InitEdge()
    {
        // �������� �ѹ��� �ش��ϴ� �����Ϳ� ���� �� ����
        // ������ ���̺�κ��� ���� ����
        // ���� ���� ����Ʈ adj ����
        adj = new List<int>[stageChamberNumber + 1];
        for (int i = 0; i <= stageChamberNumber; i++)
        {
            adj[i] = new List<int>();
            var curStageChamber = _StageChamberSO.ChamberInfoList[i];
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
    private void UpdateChamberStates()    // ���� ������ è�� ���� ������Ʈ
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
    #endregion
}
