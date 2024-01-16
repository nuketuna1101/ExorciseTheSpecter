using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int Energy = 3;
    private int Armor;
    private int Strength;


    // ���⿡ ������ �𸣰����� �ϴ� �ӽ÷�
    private int lastCompletedChamberNumber = 0;          public int LastCompletedChamberNumber { get { return lastCompletedChamberNumber; } set { lastCompletedChamberNumber = value; } }
    private int curSelectedChamberNumber = -1;           public int CurSelectedChamberNumber { get { return curSelectedChamberNumber; } set { curSelectedChamberNumber = value; } }
    private int curEnteredChamberNumber = -1;            public int CurEnteredChamberNumber { get { return curEnteredChamberNumber; } set { curEnteredChamberNumber = value; } }

    private List<int> accessableChamberList;

    [Header("Player Deck")]
    public List<CardInfo> PlayerDeck;




    //-------------------------------------------
    /// ��Ʋ �� ��Ȳ�̶�� ������ �غ���. �� �ڵ带 ���ӸŴ����� ������ ������ �𸣰����� �ϴ� ����
    /// 
    //------------------------------------
    
    // ��Ʋ �� �����ϸ�, �켱 �÷��̾�� �� ������ �ʱ�ȭ ����

    public void InitEnterBattle()
    {
        // (1) �⺻ ����
        // �÷��̾� ������ ����, �÷��̾� ������ �ε��ؿͼ� ���ε�
        // enemy�� �������� �۾�
        // ī�� ����: �÷��̾��� ���� �� �ε�

        // (2) ��: �÷��̾� ������ ����
        // �� ���� �� ��� ������ �Ҵ�.
        // �⺻ ��ο츸ŭ ī�� �ڵ�� ��ο�


        // �� ���� ��ư ������ ��뿡�� �� �ѱ��.
        



    }











    
}
