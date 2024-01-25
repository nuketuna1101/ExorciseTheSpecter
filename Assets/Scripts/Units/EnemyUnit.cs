using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField]
    private Enemy _Enemy;

    [Header("PlayerUnitPrefab : texts")]
    [SerializeField] private TMP_Text text_HP;
    [SerializeField] private TMP_Text text_Armor;
    [SerializeField] private TMP_Text text_SpellAdapt;
    [SerializeField] private TMP_Text text_Strength;
    [SerializeField] private TMP_Text text_Intelligence;
    [SerializeField] private TMP_Text text_Name;

    #region ���� �ʱ�ȭ, �ؽ�Ʈ ������
    public void InitUnit(Enemy _Enemy)           // ���� ��ü ������ ����ֱ�
    {
        this._Enemy = _Enemy;
        RefreshTexts();
    }

    public void RefreshTexts()                           // ���� ������ �ؽ�Ʈ ������Ʈ
    {
        text_HP.text = _Enemy.curHP + "/" + _Enemy.maxHp;
        text_Armor.text = _Enemy.Armor.ToString();
        text_SpellAdapt.text = _Enemy._SpellAdaptability.ToString();
        text_Strength.text = _Enemy.strength.ToString();
        text_Intelligence.text = _Enemy.intelligence.ToString();
        //text_Name.text = _Enemy.name.ToString();
    }
    #endregion
}
