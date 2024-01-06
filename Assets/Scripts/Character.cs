using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    [Header("Player : Battle Data")]
    // 체력, 방어도, 주문적응력
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // 강력함, 총명, 침착
    private int strength;
    private int intelligence;
    private int composure;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    // 축복, 저주
    private List<int> Blessings;
    private List<int> Curses;
}

public class Enemy
{
    [Header("Enemy: Indentity")]
    private int EnemyID;

    [Header("Enemy : Battle Data")]
    // 체력, 방어도, 주문적응력
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // 강력함, 총명
    private int strength = 0;
    private int intelligence = 0;
    // 태세: 일반, 은신, 반격
    private Stance _Stance = Stance.Normal;
    //
    private List<int> ActionList;
}