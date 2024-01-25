using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
/// <summary>
/// BattleScene 내에 Player와 Enemy란 BattleObj 간 전투 상호작용 및 판
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    [Header("Prefabs")]     // 전투객체 생성해주기 위한 프리팹 저장
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [Header("Prefabs SpawnPoint")]     // 전투객체 생성해주기 위한 프리팹 저장
    private readonly Vector3 spawnPoint_player = new Vector3(-9f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy1 = new Vector3(0f, 1f, -5f);
    private readonly Vector3 spawnPoint_enemy2 = new Vector3(4.5f, 1f, -5f);
    private readonly Vector3 spawnPoint_enemy3 = new Vector3(9f, 1f, -5f);


    //private GameObject PlayerGO;
    //private List<GameObject> EnemyGOs;

    private PlayerUnit playerUnit;
    private List<EnemyUnit> enemyUnits;




    [SerializeField] private readonly UnitInfo PlayerUnitInfoSO_Initial;                // 플레이어 데이터 초기화값
    [SerializeField] private UnitInfo PlayerUnitInfoSO_Current;                // 플레이어 데이터 계속 쓰는 값

    [Header("Turn Variables")]    // 턴 관련 변수
    private bool isPlayerTurn;
    private int currentTurn;

    // 드로우할 카드 변수 일단 임시
    private int initDrawCount = 5;

    // 임시 액션 델리게이트
    public static Action<bool> OnDrawCard;

    private readonly WaitForSeconds wfs1 = new WaitForSeconds(1.0f);
    private readonly WaitForSeconds wfs25 = new WaitForSeconds(0.25f);


    protected void Awake()                          // 배틀 씬 로딩 시 이벤트 달아주기 위한 델리게이트 추가
    {
        base.Awake();
        SceneManager.sceneLoaded += EnterBattleScene;
    }

    #region 배틀 씬 입장

    private void EnterBattleScene(Scene scene, LoadSceneMode mode)             // 배틀 씬 입장시 게임 플로우를 직접 버튼 행동으로 구현해보기.
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "4.BattleScene")
        {
            StartCoroutine(EnterBattleSceneCor());
        }
    }

    private IEnumerator EnterBattleSceneCor()
    {
        // 데이터 세팅
        SetPlayer();
        SetEnemy();

        // 턴 
        InitTurnSystem();
        // 덱 초기화
        CardManager.Instance.TestInitDeck();
        yield return wfs1;

        // 턴 보여주고
        UIManager.Instance.Popup_NotifyTurn();
        yield return wfs1;

        // 시작시 4장 드로우
        CardManager.Instance.DrawCards(4);
    }

    #endregion


    //-----------------------------------------------------------------------------------
    #region 각종 세팅
    public void SetPlayer()                 // 플레이어  설정
    {
        /*
        PlayerGO = Instantiate(PlayerPrefab);                          // 풀링으로 나중에 교체
        PlayerGO.transform.position = spawnPoint_player;
        _Player = new Player();
        _Player.InitProfile(PlayerUnitInfoSO_Current);
        PlayerGO.GetComponent<PlayerUnit>().InitUnit(_Player);
        */
        var PlayerGO = Instantiate(PlayerPrefab);                          // 풀링으로 나중에 교체
        PlayerGO.transform.position = spawnPoint_player;
        PlayerUnitInfoSO_Current = DataManager.Instance._PlayerGameDataSO.unitInfo;
        playerUnit = PlayerGO.GetComponent<PlayerUnit>();
        playerUnit.InitPlayerUnit(DataManager.Instance._PlayerGameDataSO);
        playerUnit.InitUnitProperty();
        playerUnit.RefreshTexts();
    }
    public void SetEnemy()              // 적 설정
    {
        /*
        var enemyObj1 = Instantiate(EnemyPrefab);                           // 풀링으로 나중에 교체
        enemyObj1.transform.position = spawnPoint_enemy1;
        var enemy1 = new Enemy();
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(enemy1);
        EnemyGOs = new List<GameObject>();
        EnemyGOs.Add(enemyObj1);
        */
        var enemyGO1 = Instantiate(EnemyPrefab);
        enemyGO1.transform.position = spawnPoint_enemy1;
        //
        enemyUnits = new List<EnemyUnit>();
        enemyUnits.Add(enemyGO1.GetComponent<EnemyUnit>());
        enemyUnits[0].InitEnemyUnit(DataManager.Instance.enemyWikiSO.EnemyWikiList[0]);
        enemyUnits[0].InitUnitProperty();
        enemyUnits[0].RefreshTexts();
    }
    #endregion

    public void InitTurnSystem()            // 턴 관련하여 초기화
    {
        currentTurn = 1;
        isPlayerTurn = true;
    }

    public void ToggleTurn()                // 턴 넘기기 .. 사실상 턴 전환
    {
        isPlayerTurn = !isPlayerTurn;
    }
    //-----------------------------------------
    /// <summary>
    /// 단일 대상 적용 카드 사용할 때 대상 적용하도록 유도
    /// </summary>
    private bool isBlinking = false;
    public void StartBlinkEnemyUnits()
    {
        DebugOpt.Log("StartBlinkEnemyUnits called");
        isBlinking = true;
        StartCoroutine(BlinkEnemyUnitsCor());
    }
    public void StopBlinkEnemyUnits()
    {
        DebugOpt.Log("StopBlinkEnemyUnits called");
        isBlinking = false;
        StopCoroutine(BlinkEnemyUnitsCor());
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            enemyUnits[i].transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
    }
    private IEnumerator BlinkEnemyUnitsCor()
    {
        while (isBlinking)
        {
            //DebugOpt.Log("BlinkEnemyUnitsCor called");
            yield return wfs25;
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                enemyUnits[i].transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
            yield return wfs25;
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                enemyUnits[i].transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
        }
    }






    //------------------------------
    #region BattleUnit 메소드 다 여길 ㅗ다시 구현

    public void Attack(Unit unit, Unit reactorUnit, DamageType _DamageType, int _DamageValue)
    {
        DebugOpt.Log("method Attack called from  " + this);
        int CalculatedDamageValue = _DamageValue;
        if (_DamageType == DamageType.Physical)
        {
            // 힘만큼 추가 데미지, 상태이상'Exhausted' 시 데미지 경감
            CalculatedDamageValue += unit.strength;

        }
        else if (_DamageType == DamageType.Magical)
        {
            // 지능만큼 추가 데미지, 상태이상'Dizzy' 시 데미지 경감
            CalculatedDamageValue += unit.intelligence;
        }
        BeAttacked(reactorUnit, _DamageType, CalculatedDamageValue);
    }
    private void BeAttacked(Unit unit, DamageType _DamageType, int CalculatedDamageValue)
    {
        // 계산된 데미지만큼 피격
        DebugOpt.Log("method BeAttacked called from  " + this);
        switch (_DamageType)
        {
            case DamageType.Physical:
                break;
            case DamageType.Magical:
                switch (unit._SpellAdaptability)
                {
                    case SpellAdaptability.None:
                        break;
                    case SpellAdaptability.Resist:
                        CalculatedDamageValue -= (CalculatedDamageValue / 4);
                        break;
                    case SpellAdaptability.Immune:
                        CalculatedDamageValue = 0;
                        break;
                    default:
                        break;
                }
                break;
            case DamageType.TrueDamage:
                break;
        }

        unit.curHP -= CalculatedDamageValue;


        // 최종 체력 0 이하면 사망처리
    }
    public void GetArmorReduced(Unit unit, int value)
    {
        // 방어도 깎임 적용
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        unit.Armor = (unit.Armor >= value ? unit.Armor - value : 0);
    }
    public void GiveStatusEffect(Unit unit, Unit reactorUnit, StatusEffect _StatusEffect)
    {
        // 상대에게 상태 이상 효과를 지속 턴만큼 부여
        GetStatusEffect(reactorUnit, _StatusEffect);
    }
    private void GetStatusEffect(Unit unit, StatusEffect _StatusEffect)
    {
        // 상태이상 효과 적용
        // 주의: 수치나 적용에 대한 갱신일뿐 실제 효과 적용은 나중에
        unit._StatusEffectArray.AddValue(_StatusEffect);
    }
    public void GetEffectWhenTurnStarts()
    {
        // 턴 시작 시 받는 효과 발동
        // 효과 큐에 넣어서 실행




    }
    public void GetEffectWhenTurnEnds()
    {
        // 턴 종료 시 받는 효과 발동

        // 출혈, 중독은 턴 종료 시 발동됨
    }



    #endregion











}