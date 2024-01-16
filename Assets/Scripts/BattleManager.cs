using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// BattleScene ���� Player�� Enemy�� BattleObj �� ���� ��ȣ�ۿ� �� ��
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    [Header("Prefabs")]     // ������ü �������ֱ� ���� ������ ����
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [Header("Prefabs SpawnPoint")]     // ������ü �������ֱ� ���� ������ ����
    private readonly Vector3 spawnPoint_player = new Vector3(-9f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy1 = new Vector3(0f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy2 = new Vector3(4.5f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy3 = new Vector3(9f, 1f, 0f);
    [Header("BattleObjects Data")]
    private Player _Player;
    private List<Enemy> _Enemies;

    [SerializeField] private readonly UnitInfoSO PlayerUnitInfoSO_Initial;                // �÷��̾� ������ �ʱ�ȭ��
    [SerializeField] private UnitInfoSO PlayerUnitInfoSO_Current;                // �÷��̾� ������ ��� ���� ��




    [Header("Turn Variables")]    // �� ���� ����
    private bool isPlayerTurn;
    private int currentTurn;





    public void TestMethod()
    {
        // ������ ����
        var newPlayer = Instantiate(PlayerPrefab);
        newPlayer.transform.position = spawnPoint_player;
        var enemyObj1 = Instantiate(EnemyPrefab);
        enemyObj1.transform.position = spawnPoint_enemy1;
        var enemyObj2 = Instantiate(EnemyPrefab);
        enemyObj2.transform.position = spawnPoint_enemy2;
        // ������ ����
        _Player = new Player();
        _Enemies = new List<Enemy>();
        var monster1 = new Enemy();
        var monster2 = new Enemy();
        // �����տ� ������ ���ε�
        newPlayer.GetComponent<PlayerUnit>().InitUnit(_Player);
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(monster1);
        enemyObj2.GetComponent<EnemyUnit>().InitUnit(monster2);
    }


    //-----------------------------------------------------------------------------------


    public void SetPlayer()                 // �÷��̾�  ����
    {
        var newPlayer = Instantiate(PlayerPrefab);                          // Ǯ������ ���߿� ��ü
        newPlayer.transform.position = spawnPoint_player;
        _Player = new Player();
        _Player.InitProfile(PlayerUnitInfoSO_Current);
        newPlayer.GetComponent<PlayerUnit>().InitUnit(_Player);
    }

    public void SetEnemy()              // �� ����
    {
        var enemyObj1 = Instantiate(EnemyPrefab);                           // Ǯ������ ���߿� ��ü
        enemyObj1.transform.position = spawnPoint_enemy1;
        var enemy1 = new Enemy();
        enemyObj1.GetComponent<EnemyUnit>().InitUnit(enemy1);
    }
    public void InitTurnSystem()            // �� �����Ͽ� �ʱ�ȭ
    {
        currentTurn = 1;
        isPlayerTurn = true;
    }

    public void ToggleTurn()                // �� �ѱ�� .. ��ǻ� �� ��ȯ
    {
        isPlayerTurn = !isPlayerTurn;
    }


}