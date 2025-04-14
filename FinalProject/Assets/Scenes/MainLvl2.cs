using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLvl2 : MonoBehaviour
{
    private int enemiesSpawned = 0;
    public int maxEnemies = 8;

    private int type1Spawned = 0;
    private int type2Spawned = 0;
    private bool levelCleared = false;

    public static MainLvl2 S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies;         // 0 = FlyingEye, 1 = Goblin
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

    public void SpawnEnemy()
    {
        if (enemiesSpawned >= maxEnemies) return;

        int chosenIndex;

        // Spawn 4 of each
        if (type1Spawned >= 4) {
            chosenIndex = 1;
        }
        else if (type2Spawned >= 4) {
            chosenIndex = 0;
        }
        else {
            chosenIndex = Random.Range(0, 2);
        }

        GameObject go = Instantiate(prefabEnemies[chosenIndex]);

        float spawnX = 10f;
        float spawnY;

        if (chosenIndex == 0) // Flying Eye
        {
            float[] possibleY = new float[] { 0.5f, 0.75f };
            spawnY = possibleY[Random.Range(0, possibleY.Length)];
        }
        else // Goblin
        {
            spawnY = -1.5f;
        }

        go.transform.position = new Vector3(spawnX, spawnY, 0f);

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
            Debug.Log("All enemies cleared! Reloading Scene_Lvl2...");
            SceneManager.LoadScene("End_Screen"); // Or advance to a new level later
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene("Scene_Lvl2");
    }

    void DelayedRestart()
    {
        Invoke(nameof(ReloadLevel), gameRestartDelay);
    }

    public static void HERO_DIED()
    {
        S.DelayedRestart();
    }
}
