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

        var enemyGO1 = Instantiate(EnemyPrefab);
        enemyGO1.transform.position = spawnPoint_enemy1;
        //
        enemyUnits = new List<EnemyUnit>();
        enemyUnits.Add(enemyGO1.GetComponent<EnemyUnit>());
        enemyUnits[0].InitEnemyUnit(DataManager.Instance.enemyWikiSO.EnemyWikiList[0], 0);
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
        DebugOpt.LogWarning("::Attack: type: " + _DamageType + " / value: " + _DamageValue);
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

        if (_DamageType == DamageType.Physical && unit.Armor > 0)
        {
            CalculatedDamageValue /= 2;
            int damageRemain = Mathf.Max(CalculatedDamageValue - unit.Armor, 0);
            GetArmorReduced(unit, CalculatedDamageValue);
            unit.curHP -= damageRemain;
        }
        else
        {
            unit.curHP -= CalculatedDamageValue;
        }
    }
    public void GetArmorReduced(Unit unit, int value)
    {
        // �� ���� ����
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        unit.Armor = Mathf.Max(unit.Armor - value, 0);
    }
    public void GiveStatusEffect(Unit unit, Unit reactorUnit, StatusEffect statusEffect)
    {
        // ��뿡�� ���� �̻� ȿ���� ���� �ϸ�ŭ �ο�
        GetStatusEffect(reactorUnit, statusEffect);
    }
    private void GetStatusEffect(Unit unit, StatusEffect statusEffect)
    {
        // �����̻� ȿ�� ����
        // ����: ��ġ�� ���뿡 ���� �����ϻ� ���� ȿ�� ������ ���߿�
        unit.statusEffectArray.AddValue(statusEffect);
    }
    private void GetOutOfStatusEffect(Unit unit, StatusEffectType statusEffectType)
    {
        unit.statusEffectArray.SetZero(statusEffectType);
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

    public void GetBuff(Unit unit, BuffType _BuffType, int value)
    {
        // 0: �߰� / 1: �� / 2: �Ѹ� / 3: ħ��/ 4: ī�� ��ο�/ 5: ü�� ȸ��
        switch (_BuffType)
        {
            case BuffType.Solid:
                unit.Armor += value;
                break;
            case BuffType.Overwhelm:
                unit.strength += value;
                break;
            case BuffType.Intelligent:
                unit.intelligence += value;
                break;
            case BuffType.Composure:
                ((PlayerUnit)unit).composure += value;
                break;
        }
    }
    public void PlayerDrawCard(int value)
    {
        CardManager.Instance.DrawCards(value);
    }
    public void RecoverHP(Unit unit, int value)
    {
        unit.curHP = Mathf.Max(unit.curHP + value, unit.maxHP);
    }
    #endregion



    #region CardManger�κ��� ���޹޾� ī�� ȿ��

    public void ActivateCardEfx(CardEffect cardEffect, bool isSingleTarget, int battleManagerEnemyListIndex)
    {
        //DebugOpt.Log("BattleManger:ActivateCardEfx:called");
        int typeCode = cardEffect.TypeCode;
        int effectRepeat = cardEffect.EffectRepeat;
        switch (typeCode)
        {
            case 1:             // ���� �ο�
                for(int i = 1; i <= effectRepeat; i++)
                {
                    AttackEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex);
                }
                break;          // ���� ����
            case 2:
                GetAdvantageEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex);
                break;
            case 3:             // ����� �ο� or ����
                ControlDebuffEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex, effectRepeat);
                break;
            case 4:             // �� �̿� ������ ȿ��
                break;
        }
    }
    private void AttackEfx(CardEffect cardEffect, bool isSingleTarget, int battleManagerEnemyListIndex)
    {
        //DebugOpt.Log("BattleManger:ActivateCardEfx:AttackEfx:called");
        int targetType = cardEffect.TargetType;
        int effectType = cardEffect.EffectType;
        int effectAmount = cardEffect.EffectAmount;
        switch (targetType)
        {
            case 0:     // �ڱ��ڽſ��� ����
                Attack(playerUnit, playerUnit, (DamageType)effectType, effectAmount);
                break;
            case 1:     // ������ ��뿡��
                Attack(playerUnit, enemyUnits[battleManagerEnemyListIndex], (DamageType)effectType, effectAmount);
                break;
            case 2:     // ��� ������
                foreach (var enemyUnit in enemyUnits)
                {
                    Attack(playerUnit, enemyUnit, (DamageType)effectType, effectAmount);
                }
                break;
            case 3:
                int randIndex = UnityEngine.Random.Range(0, enemyUnits.Count - 1);
                Attack(playerUnit, enemyUnits[randIndex], (DamageType)effectType, effectAmount);
                break;
        }
    }

    private void GetAdvantageEfx(CardEffect cardEffect, bool isSingleTarget, int battleManagerEnemyListIndex)
    {
        //DebugOpt.Log("BattleManger:ActivateCardEfx:GetAdvantageEfx:called");
        //int typeCode = cardEffect.TypeCode;
        //int targetType = cardEffect.TargetType;
        int effectType = cardEffect.EffectType;
        int effectAmount = cardEffect.EffectAmount;
        //int effectRepeat = cardEffect.EffectRepeat;

        switch (effectType)
        {
            case 0 or 1 or 2 or 3:             // ���� ������ ����
                GetBuff(playerUnit, (BuffType)effectType, effectAmount);
                break;
            case 4:             // ī�� ��ο�
                PlayerDrawCard(effectAmount);
                break;
            case 5:             // ü�� ȸ��
                RecoverHP(playerUnit, effectAmount);
                break;
        }
    }
    private void ControlDebuffEfx(CardEffect cardEffect, bool isSingleTarget, int battleManagerEnemyListIndex, int controlFlag)
    {
        //DebugOpt.Log("BattleManger:ActivateCardEfx:ControlDebuffEfx:called");
        //int typeCode = cardEffect.TypeCode;
        int targetType = cardEffect.TargetType;
        int effectType = cardEffect.EffectType;
        int effectAmount = cardEffect.EffectAmount;
        //int effectRepeat = cardEffect.EffectRepeat;
        StatusEffect statusEffect = new StatusEffect((StatusEffectType)effectType, effectAmount);

        if (controlFlag == 1)           // ����� �ο�
        {
            switch (targetType)
            {
                case 0:     // �ڱ��ڽſ��� ����
                    GiveStatusEffect(playerUnit, playerUnit, statusEffect);
                    break;
                case 1:     // ������ ��뿡��
                    GiveStatusEffect(playerUnit, enemyUnits[battleManagerEnemyListIndex], statusEffect);
                    break;
                case 2:     // ��� ������
                    foreach (var enemyUnit in enemyUnits)
                    {
                        GiveStatusEffect(playerUnit, enemyUnit, statusEffect);
                    }
                    break;
                case 3:
                    int randIndex = UnityEngine.Random.Range(0, enemyUnits.Count - 1);
                    GiveStatusEffect(playerUnit, enemyUnits[randIndex], statusEffect);
                    break;
            }
        }
        else if (controlFlag == 2)          // �ڽ��� ����� ����
        {
            GetOutOfStatusEffect(playerUnit, (StatusEffectType)effectType);
        }
    }


    #endregion







}