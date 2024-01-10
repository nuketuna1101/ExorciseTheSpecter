using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BattleScene 내에 Player와 Enemy란 BattleObj 간 전투 상호작용 및 판
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    //
    private Player _Player;
    private List<Enemy> _Enemies;

    // 턴 관련 변수
    private bool isPlayerTurn = true;
    private int totalTurnCount = 1;


    private void Awake()
    {
        // player와 enemy 할당
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
            // 물리피해 계산식 : 데미지 + strength기반 추가 데미지
            // 방어도가 있으면 절반 깎은 데미지.
            int CalculatedDamageValue = _DamageValue + this.strength;
            _Object.BeAttacked(_DamageType, CalculatedDamageValue);
        }
        else if (_DamageType == DamageType.Magical)
        {
            // 마법피해 계산식 : 데미지 + intelligence 기반 추가 데미지
            // 주문적응도에 따라 차등
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
            // 고정 데미지. 모든 수치 무시하고 수치 그대로 체력 적용
            _Object.BeAttacked(_DamageType, _DamageValue);
        }
        else
        {
            // 에러
            DebugOpt.LogWarning("ERROR :: DamageType Exception");
        }
        */
