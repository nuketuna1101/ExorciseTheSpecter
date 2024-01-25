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
    [Header("Player Game Data")]
    [SerializeField] public PlayerGameDataSO _PlayerGameDataSO;

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
    [SerializeField] private UnitInfo PlayerUnitInfoSO_Init;
    [SerializeField] private UnitInfo PlayerUnitInfoSO_Current;


    [SerializeField] public EnemyWikiSO enemyWikiSO;            // 일단 public으로 접근시켜보기. 테스트 레거시.


    protected new void Awake()
    {
        base.Awake();
        //
        ResetChamberInfo();
        SetChamberInfo();

        //
        ResetEnemyUnitInfoSO();
        SetEnemyUnitInfoSO();

        //
        ResetCardInfo();
        SetCardInfo();

        InitEdge();

        StartCoroutine(UpdateChamberStateCrtn());
    }


    #region Player GameData 유저의 게임데이터 세이브와 로딩
    private void SaveGameData()         // 세이브
    {
        //_PlayerGameDataSO.characterCode
    }
    #endregion


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

    #region [Card 관련]
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
            List <CardEffect> _CardEffectList = new List<CardEffect>();
            int TypeCode1 = CSVReader.GetIntValue(dataList, i, "TypeCode1");
            _CardEffectList.Add(new CardEffect
            {
                TypeCode = TypeCode1,
                TargetType = CSVReader.GetIntValue(dataList, i, "TargetType1"),
                EffectType = CSVReader.GetIntValue(dataList, i, "EffectType1"),
                EffectAmount = CSVReader.GetIntValue(dataList, i, "EffectAmount1"),
                EffectRepeat = CSVReader.GetIntValue(dataList, i, "EffectRepeat1"),
            });
            int? TypeCode2 = CSVReader.GetIntValue(dataList, i, "TypeCode2");
            if (TypeCode2 != -1)
            {
                _CardEffectList.Add(new CardEffect
                {
                    TypeCode = (int)TypeCode2,
                    TargetType = CSVReader.GetIntValue(dataList, i, "TargetType2"),
                    EffectType = CSVReader.GetIntValue(dataList, i, "EffectType2"),
                    EffectAmount = CSVReader.GetIntValue(dataList, i, "EffectAmount2"),
                    EffectRepeat = CSVReader.GetIntValue(dataList, i, "EffectRepeat2"),
                });
            }

            _CardInfoSO.CardInfoList.Add(new CardInfo
            {
                Class = CSVReader.GetIntValue(dataList, i, "Class"),
                CardID = CSVReader.GetIntValue(dataList, i, "CardID"),
                CardType = CSVReader.GetIntValue(dataList, i, "CardType"),
                CardCost = CSVReader.GetIntValue(dataList, i, "CardCost"),
                CardName = CSVReader.GetString(dataList, i, "CardName"),
                CardDescription = CSVReader.GetString(dataList, i, "CardContent"),
                CardEffectList = _CardEffectList,
            });
        }
    }
    #endregion

    #region [Chamber의 상태 관련]    // 현재 플레이어가 위치한 챔버 번호 (0: 처음 스테이지 최초 진입.)
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

    #region [EnemyUnitInfoSO 전투 대상 몹 데이터 관련]
    private void ResetEnemyUnitInfoSO()
    {
        //DebugOpt.Log("test log");
        enemyWikiSO.EnemyWikiList.Clear();
    }

    private void SetEnemyUnitInfoSO()
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "EnemyWiki";
        dataList = CSVReader.Read(file);
        //
        int TotalEnemyNumber = CSVReader.GetLinesLength(file) - 2;
        for (int i = 0; i < TotalEnemyNumber; i++)
        {
            enemyWikiSO.EnemyWikiList.Add(new EnemyWiki
            {
                StageNumber = CSVReader.GetIntValue(dataList, i, "StageNumber"),
                EnemyCode = CSVReader.GetIntValue(dataList, i, "EnemyCode"),
                EnemyName = CSVReader.GetString(dataList, i, "EnemyName"),
                isBoss = CSVReader.GetIntValue(dataList, i, "isBoss"),
                unitInfo = new UnitInfo
                {
                    HP = CSVReader.GetIntValue(dataList, i, "HP"),
                    Armor = CSVReader.GetIntValue(dataList, i, "Armor"),
                    _SpellAdaptability = (SpellAdaptability)CSVReader.GetIntValue(dataList, i, "SpellAdaptability"),
                    strength = CSVReader.GetIntValue(dataList, i, "strength"),
                    intelligence = CSVReader.GetIntValue(dataList, i, "intelligence"),
                },
            });
        }
    }


    #endregion
}
