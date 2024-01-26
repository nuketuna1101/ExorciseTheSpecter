using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public enum EfxType { GainArmor = 0, LoseArmor, RecoverHP, LoseHP, GetBuff, GetDebuff }

public class EfxManager : Singleton<EfxManager>
{
    [SerializeField] private GameObject GO;             // ≈ÿΩ∫∆Æ go
    private float time = 0;
    private const float timeConstraint = 2.0f;
    private const float coeff = 0.75f;


    public void ShowTextEfx(Vector3 pos, int amount, EfxType efxType, int extraFlag = 0)
    {
        Color myColor = Color.white;
        if (efxType != EfxType.GetDebuff)
        {
            myColor = ColorSettings.efxColors[(int)efxType];
        }
        StartCoroutine(GOEfxCor(pos, amount, myColor));
    }

    private IEnumerator GOEfxCor(Vector3 pos, int amount, Color color)
    {
        TMP_Text myText = GO.GetComponent<TMP_Text>();
        var myColor = GO.GetComponent<TMP_Text>().color;

        time = 0;
        GO.gameObject.SetActive(true);
        GO.transform.position = pos;
        myText.text = amount.ToString();
        myText.color = color;

        Vector3 randDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        while (time < timeConstraint)
        {
            yield return null;
            time += Time.deltaTime;
            GO.transform.Translate(randDirection * Time.deltaTime * coeff);
            GO.transform.localScale = Vector3.one * (1 + time);
           
            myColor.a = 1 - time / 2f;
            myText.color = myColor;
        }
        GO.gameObject.SetActive(false);
    }   
}
