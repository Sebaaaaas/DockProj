using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private List<GameObject> pool;

    private void Start()
    {
        InitializePool();
    }

    // Create initial objects to fill pool
    private void InitializePool()
    {
        pool = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    // To access an object
    public GameObject GetPooledObject()
    {
        // Search for non-active(not in use) object
        foreach(GameObject o in pool)
        {
            if (!o.activeInHierarchy)
                return o;
        }

        // If no activated objects, create new one
        return CreateNewObject();
    }

    private GameObject CreateNewObject()
    {
        GameObject o = Instantiate(prefab, transform);
        o.SetActive(false);
        pool.Add(o);

        return o;
    }
}
