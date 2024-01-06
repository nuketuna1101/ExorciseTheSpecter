using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class AccessHelper
{
    /// <summary>
    /// AccessHelper ::
    /// access object through this helper
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetChild(Transform parent, string name)
    {
        // parent�� ���� children �� name�� �����ϴ� �� ����
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform curChildTransform = parent.GetChild(i);
            if (curChildTransform.name == name)
                return curChildTransform.gameObject;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static GameObject GetChildHowDeep(Transform parent, string name)
    {
        // parent�� ��� �ڼ��� �������� name�� ��ġ�ϴ� �� ��ȯ
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.name == name)
                return child.gameObject;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static TMP_Text GetFirstTextChild(Transform parent)
    {
        // parent�� ���� children �� ����Ž���ؼ� text ������Ʈ ���� �� ����
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform curChildTransform = parent.GetChild(i);
            TMP_Text curText = curChildTransform.GetComponent<TMP_Text>();
            if (curText != null)      // ������Ʈ�� ������ �ִٸ�,
                return curText;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static Slider GetSliderChild(Transform parent)
    {
        // parent�� ���� children �� ����Ž���ؼ� text ������Ʈ ���� �� ����
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform curChildTransform = parent.GetChild(i);
            Slider curSlider = curChildTransform.GetComponent<Slider>();
            if (curSlider != null)      // ������Ʈ�� ������ �ִٸ�,
                return curSlider;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static void SetChildActiveAsRadio(Transform parent, int index)
    {
        // �ε����� ��ġ�ϴ� ��°�� �ڽĸ� Ȱ��ȭ, ������ �ڽ� ��Ȱ��ȭ
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject curChildObj = parent.GetChild(i).gameObject;
            curChildObj.SetActive((i == index));
        }
    }
}
