using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<GameObject, ObjectPool<GameObject>> objectsPool = new Dictionary<GameObject, ObjectPool<GameObject>>(16);
    private static ObjectPool instance { get; set; }

    void Awake()
    {
        instance = this;
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!objectsPool.TryGetValue(prefab, out ObjectPool<GameObject> pool))
        {
            pool = new ObjectPool<GameObject>(
                () => CreateNewObject(prefab),
                go => go.SetActive(true),
                go => go.SetActive(false),
                go => Destroy(go),
                defaultCapacity: 10,  // Initial pool capacity
                maxSize: 100         // Maximum pool size to prevent unbounded growth
            );
            objectsPool.Add(prefab, pool);
        }

        return pool.Get();
    }

    GameObject CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(false);
        // Store the original prefab reference if needed for later identification
        if (newObject.TryGetComponent(out PooledObject pooled))
        {
            pooled.originalPrefab = prefab;
        }
        else
        {
            pooled = newObject.AddComponent<PooledObject>();
            pooled.originalPrefab = prefab;
        }
        return newObject;
    }

    public void ReturnToPool(GameObject obj, float killTime = 0, System.Action Reset = null)
    {
        Reset?.Invoke();
        if (killTime == 0)
        {
            HideAndStoreObject(obj);
        }
        else
        {
            StartCoroutine(DelayDestroy(obj, killTime));
        }
    }

    IEnumerator DelayDestroy(GameObject obj, float killTime)
    {
        yield return new WaitForSeconds(killTime);
        HideAndStoreObject(obj);
    }

    private void HideAndStoreObject(GameObject obj)
    {
        if (obj == null)
            return;

        // Get the original prefab reference
        if (obj.TryGetComponent(out PooledObject pooled) && pooled.originalPrefab != null)
        {
            if (objectsPool.TryGetValue(pooled.originalPrefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
            else
            {
                // This shouldn't happen normally, but handle it gracefully
                ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                    () => CreateNewObject(pooled.originalPrefab),
                    go => go.SetActive(true),
                    go => go.SetActive(false),
                    go => Destroy(go),
                    defaultCapacity: 10,
                    maxSize: 100
                );
                objectsPool.Add(pooled.originalPrefab, newPool);
                newPool.Release(obj);
            }
        }
        else
        {
            // Object wasn't pooled properly, destroy it
            Destroy(obj);
        }
    }
    
    public static ObjectPool GetInstance()
    {
        return instance;
    }
}

// Helper component to track original prefab
public class PooledObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject originalPrefab;
}