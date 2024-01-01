using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("TopBar GameData")]
    public int characterCode = -1;
    private int curHP;
    private int maxHP;
    private int Gold;
    private int StageNumber;

    [Header("InBattle Player Stats")]
    private int AP;
    private int Shield;
    private int Power;








}
