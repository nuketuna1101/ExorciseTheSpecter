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
        // parent의 직속 children 중 name에 부합하는 것 접근
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
        // parent의 모든 자손을 뒤져가며 name에 일치하는 것 반환
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
        // parent의 직속 children 중 순차탐색해서 text 컴포넌트 가진 것 접근
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform curChildTransform = parent.GetChild(i);
            TMP_Text curText = curChildTransform.GetComponent<TMP_Text>();
            if (curText != null)      // 컴포넌트를 가지고 있다면,
                return curText;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static Slider GetSliderChild(Transform parent)
    {
        // parent의 직속 children 중 순차탐색해서 text 컴포넌트 가진 것 접근
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform curChildTransform = parent.GetChild(i);
            Slider curSlider = curChildTransform.GetComponent<Slider>();
            if (curSlider != null)      // 컴포넌트를 가지고 있다면,
                return curSlider;
        }
        // cannot find
        Debug.Log("CANNOT FIND CHILD. no match");
        return null;
    }
    public static void SetChildActiveAsRadio(Transform parent, int index)
    {
        // 인덱스와 일치하는 번째의 자식만 활성화, 나머지 자식 비활성화
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject curChildObj = parent.GetChild(i).gameObject;
            curChildObj.SetActive((i == index));
        }
    }
}
