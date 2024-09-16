using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab của đối tượng
    public int poolSize = 10;       // Kích thước của pool
    private List<GameObject> pool;  // Danh sách các đối tượng trong pool

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false); // Ẩn đối tượng khi chưa dùng
            pool.Add(obj);        // Thêm vào pool
        }
    }

    // Hàm lấy đối tượng từ pool
    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true); // Kích hoạt đối tượng
                return obj;
            }
        }
        // Nếu không có đối tượng nào rảnh, tạo mới và thêm vào pool
        GameObject newObj = Instantiate(objectPrefab);
        pool.Add(newObj);
        return newObj;
    }

    // Hàm trả đối tượng về pool
    public virtual void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
