using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// ÀÓ½Ã energy °ü·Ã ÄÚµå
/// </summary>


public class Energy : MonoBehaviour
{
    #region Energy °ü·Ã ÄÚµå

    [SerializeField] private TMP_Text text_Energy;
    [SerializeField] private int energy = 3;

    public void GainEnergy(int amount)            // energy È¹µæ
    {
        StartCoroutine(GainEnergyIE(amount));
    }
    public void ConsumeEnergy(int amount)            // energy È¹µæ
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
