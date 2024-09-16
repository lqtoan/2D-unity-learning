using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;   // Prefab của đối tượng
        public int size;            // Kích thước của pool
    }

    [SerializeField] private Pool[] pools;  // Mảng các pool cho các loại prefab khác nhau
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // Ẩn đối tượng khi chưa dùng
                objectPool.Enqueue(obj); // Thêm vào queue
            }

            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    // Hàm lấy đối tượng từ pool
    public GameObject GetObject(GameObject prefab)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> objectPool))
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                obj.SetActive(true); // Kích hoạt đối tượng
                return obj;
            }

            // Nếu không có đối tượng nào rảnh, tạo mới và thêm vào pool
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
            return newObj;
        }

        Debug.LogError($"Prefab {prefab.name} không có trong pool.");
        return null;
    }

    // Hàm trả đối tượng về pool
    public void ReturnObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Attempted to return a null object to the pool.");
            return;
        }

        obj.SetActive(false);

        // Tìm loại prefab của đối tượng và trả về pool tương ứng
        foreach (var pool in pools)
        {
            if (obj.CompareTag(pool.prefab.tag)) // Giả sử tag của đối tượng là tên prefab
            {
                poolDictionary[pool.prefab].Enqueue(obj);
                return;
            }
        }

        Debug.LogError("Không tìm thấy pool phù hợp để trả đối tượng.");
    }
}
