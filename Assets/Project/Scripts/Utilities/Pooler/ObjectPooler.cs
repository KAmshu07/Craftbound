using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Queue<GameObject> objectPool;
        public Action<GameObject> onCreate;
        public Action<GameObject> onGet;
        public Action<GameObject> onReturn;
        public int activeCount;  // Track active object count
    }

    [System.Serializable]
    public class PoolInstance
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<PoolInstance> initialPools;
    public int defaultExpansionAmount = 5; // Default amount to expand the pool by

    private void Start()
    {
        foreach (var pool in initialPools)
        {
            CreatePool(pool.tag, pool.prefab, pool.size);
        }
    }

    public void CreatePool(string tag, GameObject prefab, int size, Action<GameObject> onCreate = null, Action<GameObject> onGet = null, Action<GameObject> onReturn = null)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} already exists.");
            return;
        }

        Pool pool = new Pool
        {
            tag = tag,
            prefab = prefab,
            size = size,
            onCreate = onCreate,
            onGet = onGet,
            onReturn = onReturn,
            objectPool = new Queue<GameObject>(),
            activeCount = 0  // Initialize active count
        };

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            pool.onCreate?.Invoke(obj);
            pool.objectPool.Enqueue(obj);
        }

        poolDictionary.Add(tag, pool);
    }

    public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        var pool = poolDictionary[tag];

        if (pool.objectPool.Count == 0)
        {
            ExpandPool(tag, defaultExpansionAmount);
        }

        GameObject objectToSpawn = pool.objectPool.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        pool.onGet?.Invoke(objectToSpawn);
        pool.activeCount++;  // Increment active count

        return objectToSpawn;
    }

    public void ReturnObject(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].onReturn?.Invoke(obj);
        poolDictionary[tag].activeCount--;  // Decrement active count

        poolDictionary[tag].objectPool.Enqueue(obj);
    }

    public void ExpandPool(string tag, int amount)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return;
        }

        var pool = poolDictionary[tag];

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            pool.onCreate?.Invoke(obj);
            pool.objectPool.Enqueue(obj);
        }

        pool.size += amount;  // Update pool size
    }

    public Dictionary<string, Pool> GetActivePools()
    {
        return poolDictionary;
    }
}
