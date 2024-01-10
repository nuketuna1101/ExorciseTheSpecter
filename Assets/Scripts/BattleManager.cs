using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BattleScene ���� Player�� Enemy�� BattleObj �� ���� ��ȣ�ۿ� �� ��
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //
    private Player _Player;
    private List<Enemy> _Enemies;

    // �� ���� ����
    private bool isPlayerTurn = true;
    private int totalTurnCount = 1;


    private void Awake()
    {
        // player�� enemy �Ҵ�
        _Player = new Player();
        _Enemies = new List<Enemy>();
        var monster1 = new Enemy();
        var monster2 = new Enemy();
        _Enemies.Clear();
        _Enemies.Add(monster1);
        _Enemies.Add(monster2);
        //_Enemies[0].LogMyStatsForTest();
    }

    public void TestCode()
    {
        _Player.Attack(_Enemies[0], DamageType.Physical, 10);
        _Enemies[0].LogMyStatsForTest();
    }

}

            /*
        if (_DamageType == DamageType.Physical)
        {
            // �������� ���� : ������ + strength��� �߰� ������
            // ���� ������ ���� ���� ������.
            int CalculatedDamageValue = _DamageValue + this.strength;
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);
        }
        else if (_DamageType == DamageType.Magical)
        {
            // �������� ���� : ������ + intelligence ��� �߰� ������
            // �ֹ��������� ���� ����
            int CalculatedDamageValue = _DamageValue + this.intelligence;
            
            switch (_SpellAdaptability) 
            {
                case SpellAdaptability.None:
                    break;
                case SpellAdaptability.SpellResist:
                    CalculatedDamageValue -= (CalculatedDamageValue / 4);
                    break;
                case SpellAdaptability.SpellImmune:
                    CalculatedDamageValue = 0;
                    break;
                default:
                    break;
            }
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);

        }
        else if (_DamageType == DamageType.TrueDamage)
        {
            // ���� ������. ��� ��ġ �����ϰ� ��ġ �״�� ü�� ����
            _Object.BeAttacked(_DamageType, _DamageValue);
        }
        else
        {
            // ����
            DebugOpt.LogWarning("ERROR :: DamageType Exception");
        }
        */
