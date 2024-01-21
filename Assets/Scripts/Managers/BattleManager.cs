using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
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
    private readonly Vector3 spawnPoint_enemy1 = new Vector3(0f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy2 = new Vector3(4.5f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy3 = new Vector3(9f, 1f, 0f);
    [Header("BattleObjects Data")]
    private Player _Player;
    private List<Enemy> _Enemies;

    [SerializeField] private readonly UnitInfoSO PlayerUnitInfoSO_Initial;                // 플레이어 데이터 초기화값
    [SerializeField] private UnitInfoSO PlayerUnitInfoSO_Current;                // 플레이어 데이터 계속 쓰는 값

    [Header("Turn Variables")]    // 턴 관련 변수
    private bool isPlayerTurn;
    private int currentTurn;

    // 드로우할 카드 변수 일단 임시
    private int initDrawCount = 5;

    // 임시 액션 델리게이트
    public static Action<bool> OnDrawCard;

    private readonly WaitForSeconds wfs1 = new WaitForSeconds(1.0f);


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
        var newPlayer = Instantiate(PlayerPrefab);                          // 풀링으로 나중에 교체
        newPlayer.transform.position = spawnPoint_player;
        _Player = new Player();
        _Player.InitProfile(PlayerUnitInfoSO_Current);
        newPlayer.GetComponent<PlayerUnit>().InitUnit(_Player);
    }
    public void SetEnemy()              // 적 설정
    {
        var enemyObj1 = Instantiate(EnemyPrefab);                           // 풀링으로 나중에 교체
        enemyObj1.transform.position = spawnPoint_enemy1;
        var enemy1 = new Enemy();
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(enemy1);
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

}