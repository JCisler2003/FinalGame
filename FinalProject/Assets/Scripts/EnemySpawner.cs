using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public int maxEnemies = 5;

    [Header("Spawn Area")]
    public float spawnX = 12f;
    public float spawnXLeft = -12f;
    public Vector2 spawnYRange = new Vector2(-3f, 3f);

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || activeEnemies.Count >= maxEnemies) return;

        float y = Random.Range(spawnYRange.x, spawnYRange.y);
        //float x = (Random.value < 0.5f) ? spawnXLeft : spawnX;
        Vector3 spawnPosition = new Vector3(spawnX, y, 0f);
       // Vector3 spawnPosition = new Vector3(x, y, 0f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);

        // Attach tracking component
        enemy.AddComponent<EnemyMarker>().spawner = this;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    // Inner helper class
    private class EnemyMarker : MonoBehaviour
    {
        public EnemySpawner spawner;

        void OnDestroy()
        {
            if (spawner != null)
            {
                spawner.RemoveEnemy(this.gameObject);
            }
        }
    }
}