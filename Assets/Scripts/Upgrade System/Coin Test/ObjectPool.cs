using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab; // Put goldcoin as prefab here.
    public int initialPoolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitializePool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializePool()
    {
        for (int i = 0; i< initialPoolSize; i++) 
        { 
            GameObject newObj = Instantiate(objectPrefab);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(objectPrefab);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
