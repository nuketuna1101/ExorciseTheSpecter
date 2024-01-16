using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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





    public void TestMethod()
    {
        // 프리팹 생성
        var newPlayer = Instantiate(PlayerPrefab);
        newPlayer.transform.position = spawnPoint_player;
        var enemyObj1 = Instantiate(EnemyPrefab);
        enemyObj1.transform.position = spawnPoint_enemy1;
        var enemyObj2 = Instantiate(EnemyPrefab);
        enemyObj2.transform.position = spawnPoint_enemy2;
        // 데이터 생성
        _Player = new Player();
        _Enemies = new List<Enemy>();
        var monster1 = new Enemy();
        var monster2 = new Enemy();
        // 프리팹에 데이터 바인드
        newPlayer.GetComponent<PlayerUnit>().InitUnit(_Player);
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(monster1);
        enemyObj2.GetComponent<EnemyUnit>().InitUnit(monster2);
    }


    //-----------------------------------------------------------------------------------


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
    public void InitTurnSystem()            // 턴 관련하여 초기화
    {
        currentTurn = 1;
        isPlayerTurn = true;
    }

    public void ToggleTurn()                // 턴 넘기기 .. 사실상 턴 전환
    {
        isPlayerTurn = !isPlayerTurn;
    }


}