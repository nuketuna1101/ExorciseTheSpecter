using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 유틸리티 클래스
/// 자주 사용될 수 있는 함수 사전 정의.. 주로 접근 함수
/// </summary>

public class MyUtils
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object //(최상위부모, 이름 입력했는지,재귀적 검색을 할것인지(자식검색))
    {
        if (go == null)
            return null;
        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component == null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name) //이름이 비어있거나 원하는 이름이면 도출
                    return component;
            }
        }
        return null;
    }

    public static GameObject FindChildObj(GameObject parent, string name)       // 부모obj에서 이름으로 바로 아래단계 자식obj 접근. 비활성화되어도 가능
    {
        int childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = parent.transform.GetChild(i);
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
}

[Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }   
}
