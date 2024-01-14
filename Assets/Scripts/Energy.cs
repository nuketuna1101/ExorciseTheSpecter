using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// �ӽ� energy ���� �ڵ�
/// </summary>


public class Energy : MonoBehaviour
{
    #region Energy ���� �ڵ�

    [SerializeField] private TMP_Text text_Energy;
    [SerializeField] private int energy = 3;

    public void GainEnergy(int amount)            // energy ȹ��
    {
        StartCoroutine(GainEnergyIE(amount));
    }
    public void ConsumeEnergy(int amount)            // energy ȹ��
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
            energy++;
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
            energy--;
            UpdateEnergyUI();
            yield return null;
        }
    }
    private void UpdateEnergyUI()
    {
        text_Energy.text = energy.ToString();
    }
    #endregion

}
