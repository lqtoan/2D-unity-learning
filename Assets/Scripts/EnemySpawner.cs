using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Prefab của enemy
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 5; 
    private int currentEnemyCount = 0;

    private float timeSinceLastSpawn;


    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    public void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }


    private void SpawnEnemy()
    {
    if (currentEnemyCount < maxEnemies)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

  // Lấy đối tượng enemy từ pool
            GameObject enemy = objectPool.GetObject(enemyPrefab);
            if (enemy != null)
            {
                // Đặt vị trí và góc quay cho đối tượng
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;

                // Tăng số lượng kẻ thù hiện tại
                currentEnemyCount++;
            }
            else
            {
                Debug.LogError("Failed to get enemy from pool.");
            }
        }
    }
}
