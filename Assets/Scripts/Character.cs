using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    [Header("Player : Battle Data")]
    // ü��, ��, �ֹ�������
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�, ħ��
    private int strength;
    private int intelligence;
    private int composure;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    // �ູ, ����
    private List<int> Blessings;
    private List<int> Curses;
}

public class Enemy
{
    [Header("Enemy: Indentity")]
    private int EnemyID;

    [Header("Enemy : Battle Data")]
    // ü��, ��, �ֹ�������
    private int maxHp;
    private int curHP;
    private int Armor;
    private SpellAdaptability _SpellAdaptability;
    // ������, �Ѹ�
    private int strength = 0;
    private int intelligence = 0;
    // �¼�: �Ϲ�, ����, �ݰ�
    private Stance _Stance = Stance.Normal;
    //
    private List<int> ActionList;
}