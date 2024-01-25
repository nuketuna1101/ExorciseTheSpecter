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
        DebugOpt.LogWarning("::Attack: type: " + _DamageType + " / value: " + _DamageValue);
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
        // 방어도 깎임 적용
        DebugOpt.Log("method GetArmorReduced called from  " + this);
        unit.Armor = Mathf.Max(unit.Armor - value, 0);
    }
    public void GiveStatusEffect(Unit unit, Unit reactorUnit, StatusEffect statusEffect)
    {
        // 상대에게 상태 이상 효과를 지속 턴만큼 부여
        GetStatusEffect(reactorUnit, statusEffect);
    }
    private void GetStatusEffect(Unit unit, StatusEffect statusEffect)
    {
        // 상태이상 효과 적용
        // 주의: 수치나 적용에 대한 갱신일뿐 실제 효과 적용은 나중에
        unit.statusEffectArray.AddValue(statusEffect);
    }
    private void GetOutOfStatusEffect(Unit unit, StatusEffectType statusEffectType)
    {
        unit.statusEffectArray.SetZero(statusEffectType);
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

    public void GetBuff(Unit unit, BuffType _BuffType, int value)
    {
        // 0: 견고 / 1: 힘 / 2: 총명 / 3: 침착/ 4: 카드 드로우/ 5: 체력 회복
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



    #region CardManger로부터 전달받아 카드 효과

    public void ActivateCardEfx(CardEffect cardEffect, bool isSingleTarget, int battleManagerEnemyListIndex)
    {
        //DebugOpt.Log("BattleManger:ActivateCardEfx:called");
        int typeCode = cardEffect.TypeCode;
        int effectRepeat = cardEffect.EffectRepeat;
        switch (typeCode)
        {
            case 1:             // 피해 부여
                for(int i = 1; i <= effectRepeat; i++)
                {
                    AttackEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex);
                }
                break;          // 버프 적용
            case 2:
                GetAdvantageEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex);
                break;
            case 3:             // 디버프 부여 or 제거
                ControlDebuffEfx(cardEffect, isSingleTarget, battleManagerEnemyListIndex, effectRepeat);
                break;
            case 4:             // 그 이외 복잡한 효과
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
            case 0:     // 자기자신에게 피해
                Attack(playerUnit, playerUnit, (DamageType)effectType, effectAmount);
                break;
            case 1:     // 지정한 상대에게
                Attack(playerUnit, enemyUnits[battleManagerEnemyListIndex], (DamageType)effectType, effectAmount);
                break;
            case 2:     // 모든 적에게
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
            case 0 or 1 or 2 or 3:             // 버프 종류로 적용
                GetBuff(playerUnit, (BuffType)effectType, effectAmount);
                break;
            case 4:             // 카드 드로우
                PlayerDrawCard(effectAmount);
                break;
            case 5:             // 체력 회복
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

        if (controlFlag == 1)           // 디버프 부여
        {
            switch (targetType)
            {
                case 0:     // 자기자신에게 피해
                    GiveStatusEffect(playerUnit, playerUnit, statusEffect);
                    break;
                case 1:     // 지정한 상대에게
                    GiveStatusEffect(playerUnit, enemyUnits[battleManagerEnemyListIndex], statusEffect);
                    break;
                case 2:     // 모든 적에게
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
        else if (controlFlag == 2)          // 자신의 디버프 제거
        {
            GetOutOfStatusEffect(playerUnit, (StatusEffectType)effectType);
        }
    }


    #endregion







}