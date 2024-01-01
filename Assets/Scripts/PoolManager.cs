using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{ 
    private static PoolManager _instance;                          // Singleton ��������.
    [SerializeField]
    private GameObject efxPrefab;                                  // ������Ʈ ������
    [SerializeField]
    private const int initPoolSize = 50;                           // �ʱ� Ǯ ������ ����
    Queue<GameObject> pool = new Queue<GameObject>();              // ������ Ǯ�� �̿��� ť

    public static PoolManager Instance
    {
        get
        {
            if (!_instance)
            {
                // instance ���� ��� �Ҵ�.
                _instance = FindObjectOfType(typeof(PoolManager)) as PoolManager;
                if (_instance == null)
                    Debug.Log("Singleton NOT exist");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        // :: Singleton ����
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void InitPool()
    {
        for (int i = 0; i < initPoolSize; i++)
            pool.Enqueue(CreateObj());
    }
    private GameObject CreateObj()
    {
        var newObj = Instantiate(efxPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static GameObject GetEfxFromPool()
    {
        // ��û �� Ǯ�� �ִ� ������Ʈ�� �Ҵ����ش�.
        if (Instance.pool.Count > 0)
        {
            var obj = Instance.pool.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // ���� Ǯ���� �� �ʿ��ϸ�, Ǯ�� �÷� ���� �����Ͽ� �̿�
            var newObj = Instance.CreateObj();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnToPool(GameObject obj)
    {
        // ������Ʈ ��Ȱ��ȭ��Ű�� �ٽ� Ǯ�� ���ͽ�Ű��
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.pool.Enqueue(obj);
    }
}