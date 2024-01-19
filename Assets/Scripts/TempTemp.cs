using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempTemp : MonoBehaviour
{
    [SerializeField] public Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    enum imagetmps
    {
        img12,
        img123
    }

    private void Awake()
    {
        BindStatic<Image>(typeof(imagetmps));
        Debug.Log(Get<Image>((int)imagetmps.img12).GetComponent<Image>().name);

    }


    private void BindStatic<T>(Type type) where T : UnityEngine.Object
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);
        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindChild<T>(gameObject, names[i], true);
        }
    }
    private T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) return null;
        return objects[index] as T;
    }
}
