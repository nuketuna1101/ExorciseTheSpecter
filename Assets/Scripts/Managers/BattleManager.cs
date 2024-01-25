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
/// BattleScene ���� Player�� Enemy�� BattleObj �� ���� ��ȣ�ۿ� �� ��
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    [Header("Prefabs")]     // ������ü �������ֱ� ���� ������ ����
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [Header("Prefabs SpawnPoint")]     // ������ü �������ֱ� ���� ������ ����
    private readonly Vector3 spawnPoint_player = new Vector3(-9f, 1f, 0f);
    private readonly Vector3 spawnPoint_enemy1 = new Vector3(0f, 1f, -5f);
    private readonly Vector3 spawnPoint_enemy2 = new Vector3(4.5f, 1f, -5f);
    private readonly Vector3 spawnPoint_enemy3 = new Vector3(9f, 1f, -5f);


    //private GameObject PlayerGO;
    //private List<GameObject> EnemyGOs;

    private PlayerUnit playerUnit;
    private List<EnemyUnit> enemyUnits;




    [SerializeField] private readonly UnitInfo PlayerUnitInfoSO_Initial;                // �÷��̾� ������ �ʱ�ȭ��
    [SerializeField] private UnitInfo PlayerUnitInfoSO_Current;                // �÷��̾� ������ ��� ���� ��

    [Header("Turn Variables")]    // �� ���� ����
    private bool isPlayerTurn;
    private int currentTurn;

    // ��ο��� ī�� ���� �ϴ� �ӽ�
    private int initDrawCount = 5;

    // �ӽ� �׼� ��������Ʈ
    public static Action<bool> OnDrawCard;

    private readonly WaitForSeconds wfs1 = new WaitForSeconds(1.0f);
    private readonly WaitForSeconds wfs25 = new WaitForSeconds(0.25f);


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
        /*
        PlayerGO = Instantiate(PlayerPrefab);                          // Ǯ������ ���߿� ��ü
        PlayerGO.transform.position = spawnPoint_player;
        _Player = new Player();
        _Player.InitProfile(PlayerUnitInfoSO_Current);
        PlayerGO.GetComponent<PlayerUnit>().InitUnit(_Player);
        */
        var PlayerGO = Instantiate(PlayerPrefab);                          // Ǯ������ ���߿� ��ü
        PlayerGO.transform.position = spawnPoint_player;
        PlayerUnitInfoSO_Current = DataManager.Instance._PlayerGameDataSO.unitInfo;
        playerUnit = PlayerGO.GetComponent<PlayerUnit>();
        playerUnit.InitPlayerUnit(DataManager.Instance._PlayerGameDataSO);
        playerUnit.InitUnitProperty();
        playerUnit.RefreshTexts();
    }
    public void SetEnemy()              // �� ����
    {
        /*
        var enemyObj1 = Instantiate(EnemyPrefab);                           // Ǯ������ ���߿� ��ü
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
    /// <summary>
    /// ���� ��� ���� ī�� ����� �� ��� �����ϵ��� ����
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
    #region BattleUnit �޼ҵ� �� ���� �Ǵٽ� ����

    public void Attack(Unit unit, Unit reactorUnit, DamageType _DamageType, int _DamageValue)
    {
        DebugOpt.Log("method Attack called from  " + this);
        int CalculatedDamageValue = _DamageValue;
        if (_DamageType == DamageType.Physical)
        {
            // ����ŭ �߰� ������, �����̻�'Exhausted' �� ������ �氨
            CalculatedDamageValue += unit.strength;

        }
        else if (_DamageType == DamageType.Magical)
        {
            // ���ɸ�ŭ �߰� ������, �����̻�'Dizzy' �� ������ �氨
            CalculatedDamageValue += unit.intelligence;
        }
        BeAttacked(reactorUnit, _DamageType, CalculatedDamageValue);
    }
    private void BeAttacked(Unit unit, DamageType _DamageType, int CalculatedDamageValue)
    {
        // ���� ��������ŭ �ǰ�
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


        // ���� ü�� 0 ���ϸ� ���ó��
    }
    public void GetArmorReduced(Unit unit, int value)
    {
        // �� ���� ����
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        unit.Armor = (unit.Armor >= value ? unit.Armor - value : 0);
    }
    public void GiveStatusEffect(Unit unit, Unit reactorUnit, StatusEffect _StatusEffect)
    {
        // ��뿡�� ���� �̻� ȿ���� ���� �ϸ�ŭ �ο�
        GetStatusEffect(reactorUnit, _StatusEffect);
    }
    private void GetStatusEffect(Unit unit, StatusEffect _StatusEffect)
    {
        // �����̻� ȿ�� ����
        // ����: ��ġ�� ���뿡 ���� �����ϻ� ���� ȿ�� ������ ���߿�
        unit._StatusEffectArray.AddValue(_StatusEffect);
    }
    public void GetEffectWhenTurnStarts()
    {
        // �� ���� �� �޴� ȿ�� �ߵ�
        // ȿ�� ť�� �־ ����




    }
    public void GetEffectWhenTurnEnds()
    {
        // �� ���� �� �޴� ȿ�� �ߵ�

        // ����, �ߵ��� �� ���� �� �ߵ���
    }



    #endregion











}