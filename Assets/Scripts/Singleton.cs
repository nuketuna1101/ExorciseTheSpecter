using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 씬 전환에도 유지하는 자주 사용하는 싱글턴 패턴 탬플릿
    private static T instance;

    public static T Instance
    {
        get
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null)
            {
                var ob = new GameObject(typeof(T).Name, typeof(T));
                instance = ob.GetComponent<T>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
