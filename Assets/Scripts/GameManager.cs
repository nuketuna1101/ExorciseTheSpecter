using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : Singleton<GameManager>
{
    [Header("TopBar GameData")]
    private int characterCode = -1;                 public int CharacterCode { get { return characterCode; } set { characterCode = value; } }
    private int curHP = 25;                          public int CurHP { get { return curHP; } set { curHP = value; } }
    private int maxHP = 100;                          public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    private int gold = 250;                           public int Gold { get { return gold; } set { gold = value; } }
    private int stageNumber = 1;                    public int StageNumber { get { return stageNumber; } set { stageNumber = value; } }

    [Header("InBattle Player Stats")]
    private int AP;
    private int Shield;
    private int Power;


    // 여기에 있을지 모르겟지만 일단 임시로
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


}
