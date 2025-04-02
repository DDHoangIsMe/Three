using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToPool;
    private Type typeToPool;
    public int amountToPool = 0;
    public bool shouldExpand = true;
    private List<GameObject> pooledObjects = new List<GameObject>();

    public void GetTypeObj<T>()
    {
        typeToPool = typeof(T);
        objectToPool = Resources.Load<GameObject>("Prefabs/Prefab" + typeToPool.Name);
    }

    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.AddComponent(typeToPool);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (shouldExpand)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.AddComponent(typeToPool);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private string DatalizeObject<T, Y>(GameObject a) where T : ITarget<Y> where Y : IData
    {
        // Construct: type`name+x+y+z
        T data = a.GetComponent<T>();
        return data.GetData().GetName() + "+" +
               a.transform.position.x + "+" +
               a.transform.position.y + "+" +
               a.transform.position.z;
    }
    public StringBuilder GetAllObject<T, Y>() where T : ITarget<Y> where Y : IData
    {
        StringBuilder result = new StringBuilder();
        foreach (GameObject item in pooledObjects) 
        {
            result.Append("`");
            result.Append(DatalizeObject<T, Y>(item));
        }
        return result;
    }

    //
    public void ClearAll()
    {
        foreach (GameObject item in pooledObjects)
        {
            item.SetActive(false);
        }
    }
}
