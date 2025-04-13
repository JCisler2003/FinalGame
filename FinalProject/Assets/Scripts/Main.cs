using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private int enemiesSpawned = 0;
    public int maxEnemies = 6;
    private bool levelCleared = false;

    static private Main S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2;

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    void Update()
    {
        // When all enemies are spawned and none are left, load next level
        if (!levelCleared && enemiesSpawned >= maxEnemies && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            levelCleared = true;
            Debug.Log("All enemies defeated! Advancing to Scene_Lvl2...");
            Invoke(nameof(LoadNextLevel), 2f); // wait 2 seconds
        }
    }

    public void SpawnEnemy()
    {
        if (enemiesSpawned >= maxEnemies) return;

        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]);

        float spawnX = 10f;
        float[] possibleY = new float[] { 0.5f, 0.75f };
        float spawnY = possibleY[Random.Range(0, possibleY.Length)];

        Vector3 pos = new Vector3(spawnX, spawnY, 0f);
        go.transform.position = pos;

        enemiesSpawned++;

        if (enemiesSpawned < maxEnemies)
        {
            Invoke(nameof(SpawnEnemy), 3f);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene("Scene_Lvl2");
    }

    void DelayedRestart()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene("Scene_0");
    }

    static public void HERO_DIED()
    {
        S.DelayedRestart();
    }
}
