using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BattleScene 내에 Player와 Enemy란 BattleObj 간 전투 상호작용 및 판
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    [Header("Prefabs")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;

    private readonly Vector3 spawnPoint_player = new Vector3(-9f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy1 = new Vector3(0f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy2 = new Vector3(4.5f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy3 = new Vector3(9f, 1f, 0f);

    //
    private Player _Player;
    private List<Enemy> _Enemies;

    // 턴 관련 변수
    private bool isPlayerTurn = true;
    private int totalTurnCount = 1;

    public void TestCode()
    {
        /*
// player와 enemy 할당
_Player = new Player();
_Enemies = new List<Enemy>();
var monster1 = new Enemy();
var monster2 = new Enemy();
_Enemies.Clear();
_Enemies.Add(monster1);
_Enemies.Add(monster2);
*/
        //_Enemies[0].LogMyStatsForTest();
        _Player.Attack(_Enemies[0], DamageType.Physical, 10);
        _Enemies[0].LogMyStatsForTest();
    }


    public void TestMethod()
    {
        var newPlayer = Instantiate(PlayerPrefab);
        newPlayer.transform.position = spawnPoint_player;

        var enemyObj1 = Instantiate(EnemyPrefab);
        enemyObj1.transform.position = spawnPoint_enemy1;

        var enemyObj2 = Instantiate(EnemyPrefab);
        enemyObj2.transform.position = spawnPoint_enemy2;

        _Player = new Player();
        _Enemies = new List<Enemy>();
        var monster1 = new Enemy();
        var monster2 = new Enemy();



        newPlayer.GetComponent<PlayerUnit>().InitUnit(_Player);
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(monster1);
        enemyObj2.GetComponent<EnemyUnit>().InitUnit(monster2);
    }



}