using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{


    // ĳ���� ����
    public enum SpellAdaptability { None, SpellResist, SpellImmune }
    private int HP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    //
    private int strength;
    private int intelligence;
    private int composure;

    //
    private bool isStealthMode;
    private bool isCounterAtkMode;



}
