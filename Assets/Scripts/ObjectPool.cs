using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀링 = 필요한 객체를 미리 만들어두고 꺼내 쓰는것

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public int maxObject = 30;
    List<GameObject> pool;


    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < maxObject; i++)
        {
            GameObject obj = Instantiate(prefab,parent);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject Get()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}