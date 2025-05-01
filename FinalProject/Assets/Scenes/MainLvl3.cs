using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLvl3 : MonoBehaviour
{
    private int enemiesSpawned = 0;
    public int maxEnemies = 12;

    private int type1Spawned = 0;
    private int type2Spawned = 0;
    private bool levelCleared = false;

    public static MainLvl3 S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float gameRestartDelay = 2;

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        if (enemiesSpawned >= maxEnemies) return;

        int chosenIndex = (type1Spawned >= 6) ? 1 : (type2Spawned >= 6) ? 0 : Random.Range(0, 2);

        GameObject go = Instantiate(prefabEnemies[chosenIndex]);
        float spawnY = (chosenIndex == 0) ? new float[] { 0.5f, 0.75f }[Random.Range(0, 2)] : -1.5f;
        go.transform.position = new Vector3(10f, spawnY, 0f);

        enemiesSpawned++;
        if (chosenIndex == 0) type1Spawned++;
        else type2Spawned++;

        if (enemiesSpawned < maxEnemies)
        {
            Invoke(nameof(SpawnEnemy), 3f);
        }
    }

    void Update()
    {
        if (!levelCleared && enemiesSpawned >= maxEnemies && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            levelCleared = true;
            Invoke(nameof(LoadNextLevel), 2f);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene("Scene_Lvl4");
    }

    void DelayedRestart()
    {
        Invoke(nameof(ReloadLevel), gameRestartDelay);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene("Scene_Lvl3");
    }

    public static void HERO_DIED()
    {
        if (S != null) S.DelayedRestart();
    }
}
