using UnityEngine;
using UnityEngine.SceneManagement;



public class Main : MonoBehaviour
{
    private int enemiesSpawned = 0;
    public int maxEnemies = 6;
    static private Main S; // a private singleton for Main

    //static private Dictionary<eWeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Inscribed")]
    //public bool spawnEnemies = true;
    public GameObject[] prefabEnemies; // array of enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # enemies spawned/second
    public float enemyInsetDefault = 1.5f; // inset from the sides
    public float gameRestartDelay = 2;
    // public GameObject prefabPowerUp;

    // public WeaponDefinition[] weaponDefinitions;

    // public eWeaponType[] powerUpFrequency = new eWeaponType[] {
    //     eWeaponType.blaster, eWeaponType.blaster,
    //     eWeaponType.spread, eWeaponType.shield 
    // };

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        // set bndcheck to reference the boundscheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // invoke spawnenemy() once (in 2 seconds, based on default values)
         Invoke ( nameof(SpawnEnemy), 1f/enemySpawnPerSecond );

        // WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        // foreach(WeaponDefinition def in weaponDefinitions){
        //     WEAP_DICT[def.type] = def;
        // }
    }

    // static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt){
    //     if (WEAP_DICT.ContainsKey(wt)){
    //         return (WEAP_DICT[wt]);
    //     }

    //     return(new WeaponDefinition());
    // }

    // static public void SHIP_DESTROYED( Enemy e ) {
    //     // potentially generate a PowerUp
    //     if (Random.value <= e.powerUpDropChance) {
    //         int ndx = Random.Range( 0, S.powerUpFrequency.Length );
    //         eWeaponType pUpType = S.powerUpFrequency[ndx];

    //         // spawn a powerup
    //         GameObject go = Instantiate<GameObject>( S.prefabPowerUp );
    //         PowerUp pUp = go.GetComponent<PowerUp>();
    //         // set it to the proper WeaponType
    //         pUp.SetType( pUpType );

    //         // set it to the position
    //         pUp.transform.position = e.transform.position;
    //     }
    // }

    public void SpawnEnemy()
    {
        if (enemiesSpawned >= maxEnemies) return;

        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float spawnX = 10f;
        float[] possibleY = new float[] { 0.5f, 0.75f };
        float spawnY = possibleY[Random.Range(0, possibleY.Length)];

        Vector3 pos = new Vector3(spawnX, spawnY, 0f);
        go.transform.position = pos;

        enemiesSpawned++;

        // Only keep invoking if we haven’t hit the limit
        if (enemiesSpawned < maxEnemies)
        {
            Invoke(nameof(SpawnEnemy), 3f);
        }
    }
    void DelayedRestart() {
        // invoke the Restart() method in gameRestartDelay seconds
        Invoke( nameof(Restart), gameRestartDelay);
    }

    void Restart() {
        // reload __Scene_0 to restart the game
        SceneManager.LoadScene( "Scene_0" );
    }

    static public void HERO_DIED() {
        S.DelayedRestart();
    }
}

