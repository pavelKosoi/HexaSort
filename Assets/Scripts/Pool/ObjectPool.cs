using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public enum PoolObjectType
    {
        None,
        Hex
    }


    [System.Serializable]
    public class PoolItem
    {
        public PoolObjectType Key;
        public GameObject Prefab;
        public int InitialSize = 10;
        public Transform DefaultParent;
    }

    [SerializeField] private List<PoolItem> items;

    private Dictionary<PoolObjectType, Queue<GameObject>> pool = new();
    private Dictionary<PoolObjectType, GameObject> prefabLookup = new();
    List<Poolable> poolables = new List<Poolable>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in items)
        {
            pool[item.Key] = new Queue<GameObject>();
            prefabLookup[item.Key] = item.Prefab;

            for (int i = 0; i < item.InitialSize; i++)
                CreateNewObject(item.Key);
        }
    }

    int i;
    private GameObject CreateNewObject(PoolObjectType key)
    {
        if (!prefabLookup.ContainsKey(key))
        {
            Debug.LogError($"[Pool] No prefab registered with key '{key}'");
            return null;
        }

        Transform parent = items.FirstOrDefault(i => i.Key == key).DefaultParent;
        if (parent == null) parent = transform;
        var obj = Instantiate(prefabLookup[key], parent);
        obj.SetActive(false);

        var poolable = obj.GetComponent<Poolable>();
        if (poolable != null)
        {
            poolable.OriginPool = this;
            poolable.DefaultParent = parent;
            poolable.Init();
            poolables.Add(poolable);
        }

        obj.name += i;
        i++;
        pool[key].Enqueue(obj);
        return obj;
    }

    public Poolable GetFromPool(PoolObjectType key, Vector3 position = default, Quaternion rotation = default, bool activate = true)
    {
        if (!pool.ContainsKey(key))
        {
            Debug.LogError($"[Pool] No pool registered with key '{key}'");
            return null;
        }

        if (pool[key].Count == 0)
            CreateNewObject(key);

        var obj = pool[key].Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        if(activate) obj.SetActive(true);

        var poolable = obj.GetComponent<Poolable>();
        poolable?.OnTakenFromPool();

        return poolable;
    }

    public void BackToPool(Poolable poolable)
    {        
        if (poolable != null && poolable.OriginPool != this)
        {
            Debug.LogWarning("[Pool] Trying to return object to wrong pool.");
            if(poolable.gameObject) Destroy(poolable.gameObject);
            return;
        }

        poolable.OnReturnedToPool();
        poolable.transform.SetParent(poolable.DefaultParent);
        poolable.gameObject.SetActive(false);

        var key = FindKeyForObject(poolable.gameObject);
        if (key != null)
            pool[key.Value].Enqueue(poolable.gameObject);
    }

    public void BackEverythingToPool()
    {
        foreach (var item in poolables)
        {
           if(item != null) item.ReturnToPool();
        }
    }

    private PoolObjectType? FindKeyForObject(GameObject obj)
    {
        foreach (var kvp in prefabLookup)
        {
            if (obj.name == kvp.Value.name || obj.name.StartsWith(kvp.Value.name + "("))
                return kvp.Key;
        }

        Debug.LogWarning("[Pool] Could not find key for object: " + obj.name);
        return null;
    }

}
