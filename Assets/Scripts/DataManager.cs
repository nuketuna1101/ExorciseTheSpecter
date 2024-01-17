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
    // 인접리스트와 방문노드
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
    [SerializeField] private Sprite[] CharacterClassSprites;     // 플레이어 직업 클래스 기본 이미지 스프라이트
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

    #region Chamber 데이터 초기화 불러오기 관련
    private void ResetChamberInfo()    // StageChamberSO 초기화
    {
        _StageChamberSO.ChamberInfoList.Clear();
    }
    private void SetChamberInfo()    // CSV로부터 StageChamberSO 저장
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

    #region Card 관련
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

    #region Chamber의 상태 관련    // 현재 플레이어가 위치한 챔버 번호 (0: 처음 스테이지 최초 진입.)
    private void InitEdge()
    {
        // 스테이지 넘버에 해당하는 데이터에 따라 맵 세팅
        // 데이터 테이블로부터 사전 세팅
        // 인접 엣지 리스트 adj 세팅
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
        // 방문 배열 visited 세팅
        visited = new bool[stageChamberNumber + 1];
        Array.Fill(visited, false);
        visited[0] = true;
        // 챔버 상태 배열 초기화
        _ChamberStates = new ChamberState[stageChamberNumber + 1];
        //
    }
    private void UpdateChamberStates()    // 현재 상태의 챔버 상태 업데이트
    {
        // 기본 rest of으로 초기화
        Array.Fill(_ChamberStates, ChamberState.RestOf);
        // 방문한 챔버 업데이트
        for (int i = 0; i <= stageChamberNumber; i++)
        {
            if (visited[i])
                _ChamberStates[i] = ChamberState.Visited;
        }

        // 선택된 거 업데이트
        if (GameManager.Instance.CurSelectedChamberNumber != -1)
            _ChamberStates[GameManager.Instance.CurSelectedChamberNumber] = ChamberState.Selected;


        // 현재 상태 기반 방문 가능 업데이트
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
