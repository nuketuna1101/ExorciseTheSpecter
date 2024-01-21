using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    // ��ο��� ī�� ���� �ϴ� �ӽ�
    private int initDrawCount = 5;

    // �ӽ� �׼� ��������Ʈ
    public static Action<bool> OnDrawCard;

    private readonly WaitForSeconds wfs1 = new WaitForSeconds(1.0f);


    protected void Awake()                          // ��Ʋ �� �ε� �� �̺�Ʈ �޾��ֱ� ���� ��������Ʈ �߰�
    {
        base.Awake();
        SceneManager.sceneLoaded += EnterBattleScene;
    }

    #region ��Ʋ �� ����

    private void EnterBattleScene(Scene scene, LoadSceneMode mode)             // ��Ʋ �� ����� ���� �÷ο츦 ���� ��ư �ൿ���� �����غ���.
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "4.BattleScene")
        {
            StartCoroutine(EnterBattleSceneCor());
        }
    }

    private IEnumerator EnterBattleSceneCor()
    {
        // ������ ����
        SetPlayer();
        SetEnemy();

        // �� 
        InitTurnSystem();
        // �� �ʱ�ȭ
        CardManager.Instance.TestInitDeck();
        yield return wfs1;

        // �� �����ְ�
        UIManager.Instance.Popup_NotifyTurn();
        yield return wfs1;

        // ���۽� 4�� ��ο�
        CardManager.Instance.DrawCards(4);
    }

    #endregion


    //-----------------------------------------------------------------------------------
    #region ���� ����
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
    #endregion

    public void InitTurnSystem()            // �� �����Ͽ� �ʱ�ȭ
    {
        currentTurn = 1;
        isPlayerTurn = true;
    }

    public void ToggleTurn()                // �� �ѱ�� .. ��ǻ� �� ��ȯ
    {
        isPlayerTurn = !isPlayerTurn;
    }
    //-----------------------------------------

}