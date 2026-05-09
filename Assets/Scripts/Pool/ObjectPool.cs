using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }

    public GameObject Get()
    {
        GameObject obj;

        if (pool.Count > 0)
            obj = pool.Dequeue();
        else
            obj = CreateNewObject();

        obj.SetActive(true);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
            poolable.OnSpawn();

        return obj;
    }

    public void Return(GameObject obj)
    {
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
            poolable.OnReturnToPool();

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}