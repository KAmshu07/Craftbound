using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public string poolTag;
    public GameObject prefab;  // Prefab to pool
    public Vector3 spawnPosition;
    public int initialPoolSize = 10;

    private List<GameObject> activeObjects = new List<GameObject>();

    private void Start()
    {
        // Register custom pool
        ObjectPooler.Instance.CreatePool(poolTag, prefab, initialPoolSize, OnCreate, OnGet, OnReturn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = ObjectPooler.Instance.GetObject(poolTag, spawnPosition, Quaternion.identity);
            activeObjects.Add(obj);
        }

        if (Input.GetKeyDown(KeyCode.R) && activeObjects.Count > 0)
        {
            GameObject obj = activeObjects[0];
            activeObjects.RemoveAt(0);
            ObjectPooler.Instance.ReturnObject(poolTag, obj);
        }
    }

    private void OnCreate(GameObject obj)
    {
        // Custom initialization, if any
        Debug.Log($"Created new object: {obj.name}");
    }

    private void OnGet(GameObject obj)
    {
        // Custom actions on getting an object from the pool
        Debug.Log($"Got object from pool: {obj.name}");
    }

    private void OnReturn(GameObject obj)
    {
        // Custom actions on returning an object to the pool
        Debug.Log($"Returned object to pool: {obj.name}");
    }
}
