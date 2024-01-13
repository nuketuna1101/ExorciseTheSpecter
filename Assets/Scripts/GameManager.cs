using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("TopBar GameData")]
    private int characterCode = -1;                 public int CharacterCode { get { return characterCode; } set { characterCode = value; } }
    private int curHP = 25;                          public int CurHP { get { return curHP; } set { curHP = value; } }
    private int maxHP = 100;                          public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    private int gold = 250;                           public int Gold { get { return gold; } set { gold = value; } }
    private int stageNumber = 1;                    public int StageNumber { get { return stageNumber; } set { stageNumber = value; } }

    [Header("InBattle Player Stats")]
    private int Energy = 3;
    private int Armor;
    private int Strength;


    // ¿©±â¿¡ ÀÖÀ»Áö ¸ð¸£°ÙÁö¸¸ ÀÏ´Ü ÀÓ½Ã·Î
    private int lastCompletedChamberNumber = 0;          public int LastCompletedChamberNumber { get { return lastCompletedChamberNumber; } set { lastCompletedChamberNumber = value; } }
    private int curSelectedChamberNumber = -1;           public int CurSelectedChamberNumber { get { return curSelectedChamberNumber; } set { curSelectedChamberNumber = value; } }
    private int curEnteredChamberNumber = -1;            public int CurEnteredChamberNumber { get { return curEnteredChamberNumber; } set { curEnteredChamberNumber = value; } }

    private List<int> accessableChamberList;

    [Header("Player Deck")]
    public List<CardInfo> PlayerDeck;
















    protected new void Awake()
    {
        base.Awake();
        DebugOpt.Log("GameManager.Instance.CharacterCode :: " + GameManager.Instance.CharacterCode);
        AudioManager.Instance.PlayBGM();
    }




    public PlayerUnit _PlayerUnit;
    public EnemyUnit _EnemyUnit1;
    public EnemyUnit _EnemyUnit2;
    public EnemyUnit _EnemyUnit3;

    // test code
    public void InitUnits()
    {
        Player _Player = new Player();
        List<Enemy> _Enemies = new List<Enemy>();
        var monster1 = new Enemy();
        var monster2 = new Enemy();
        _Enemies.Clear();
        _Enemies.Add(monster1);
        _Enemies.Add(monster2);

        _PlayerUnit.InitUnit(_Player);
        _EnemyUnit1.InitUnit(monster1);
        _EnemyUnit2.InitUnit(monster2);

    }





    #region Energy °ü·Ã ÄÚµå

    [SerializeField] private TMP_Text text_Energy;

    public int GetEnergy()
    {
        return Energy;
    }
    public void GainEnergy(int amount)            // energy È¹µæ
    {
        StartCoroutine(GainEnergyIE(amount));
    }
    public void ConsumeEnergy(int amount)            // energy È¹µæ
    {
        StartCoroutine(ConsumeEnergyIE(amount));

    }

    private IEnumerator GainEnergyIE(int amount)
    {
        int loop = amount;
        while (true)
        {
            if (loop <= 0) break;
            loop--;
            Energy++;
            UpdateEnergyUI();
            yield return null;
        }
    }
    private IEnumerator ConsumeEnergyIE(int amount)
    {
        int loop = amount;
        while (true)
        {
            if (loop <= 0) break;
            loop--;
            Energy--;
            UpdateEnergyUI();
            yield return null;
        }
    }
    private void UpdateEnergyUI()
    {
        text_Energy.text = Energy.ToString();
    }
    #endregion

}
