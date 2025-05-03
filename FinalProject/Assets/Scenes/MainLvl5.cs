using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLvl5 : MonoBehaviour
{
    public static MainLvl5 S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies; // index 0: FlyingEye, index 1: Goblin
    public GameObject bossEnemy;
    public float enemySpawnPerSecond = 0.5f;
    public float gameRestartDelay = 2f;
    public int maxEnemies = 12;

    private int enemiesSpawned = 0;
    private int type1Spawned = 0;
    private int type2Spawned = 0;
    private GameObject bossInstance;
    private bool bossDefeated = false;

    void Awake()
    {
        S = this;

        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
        SpawnBoss();
    }

    void SpawnBoss()
    {
        Vector3 bossSpawnPos = new Vector3(8f, 0f, 0f); // adjust as needed
        bossInstance = Instantiate(bossEnemy, bossSpawnPos, Quaternion.identity);
    }

    void SpawnEnemy()
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
        if (!bossDefeated && bossInstance == null)
        {
            bossDefeated = true;
            Debug.Log("Boss defeated! Ending game...");
            Invoke(nameof(LoadNextScene), 2f);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("End_Screen"); // or win screen
    }

    void DelayedRestart()
    {
        Invoke(nameof(ReloadLevel), gameRestartDelay);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene("Scene_Lvl5");
    }

    public static void HERO_DIED()
    {
        if (S != null)
        {
            S.DelayedRestart();
        }
    }
}
