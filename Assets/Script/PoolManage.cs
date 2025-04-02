using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PoolManage : Singleton<PoolManage>
{
    public Dictionary<string, PoolObject> poolDictionary = new Dictionary<string, PoolObject>();
    public GameObject GetObjectFromPool<T>()
    {
        string objectType = typeof(T).Name;
        if (!poolDictionary.ContainsKey(objectType))
        {
            GeneratePool<T>();
        }
        return poolDictionary[objectType].GetObjectFromPool();
    }
    private void GeneratePool<T>()
    {
        string objectType = typeof(T).Name;
        if (!poolDictionary.ContainsKey(objectType)) {
            GameObject pool = new GameObject(objectType + "-Pool");
            pool.transform.parent = transform;
            PoolObject poolObject = pool.AddComponent<PoolObject>();
            poolObject.GetTypeObj<T>();
            poolDictionary.Add(objectType, poolObject);
        }
    }

    public StringBuilder GetAllObject<T, Y>() where T : ITarget<Y> where Y : IData
    {
        if (poolDictionary.ContainsKey(typeof(T).Name)) 
        {
            PoolObject temp = poolDictionary[typeof(T).Name];
            return temp.GetAllObject<T, Y>();
        }

        return null as StringBuilder;
    }

    public void ClearWorld<T>()
    {
        string clearType = typeof(T).Name;
        if (poolDictionary.ContainsKey(clearType)) 
        {
            poolDictionary[clearType].ClearAll();
        }
    }

    private void ClearWorld()
    {
        foreach (var item in poolDictionary) 
        {
            item.Value.ClearAll();
        }
    }
}
