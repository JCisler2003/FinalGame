//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Cowboy : MonoBehaviour
{
    static public Cowboy S { get; private set; }

    [Header("Inscribed")]
    public float speed = 10f;
    public float jumpForce = 10f;
    public float pitchMult = 30;
    
    [Header("Sword Attack Settings")]
    public Transform hitPoint;
    public BoxCollider2D hitBox;
    //public float attackRange = 1.5f;
    public Vector2 attackRange;
    public float attackDamage = 5f;
    public LayerMask enemyLayer;

    [Header("Dynamic")] [Range(0, 4)]
    private float _shieldLevel = 4; // Start with 4 hits allowed
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private Transform visual;
    private Vector3 originalScale;
    private SpriteAnimator spriteAnimator;
    

    //public BoxCollider2D hitBox;

    void Awake()
    {
        if (S == null) S = this;
        else Debug.LogError("Attempted to assign second Cowboy.S");

        rb = GetComponent<Rigidbody2D>();
        visual = transform.Find("Sprite");
        if (visual != null)
        {
            originalScale = visual.localScale;
            spriteAnimator = visual.GetComponent<SpriteAnimator>();
        }
        else
        {
            Debug.LogWarning("Visual child object 'Sprite' not found. Flip will not work.");
        }

         hitBox = hitPoint.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.linearVelocity;
        velocity.x = hAxis * speed;
        rb.linearVelocity = velocity;

        if (visual != null && Mathf.Abs(hAxis) > 0.01f)
        {
            spriteAnimator.FlipSprite(hAxis);

            Vector3 HitPointPos = hitPoint.localPosition;
            HitPointPos.x *= -1;
            hitPoint.localPosition = HitPointPos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            spriteAnimator.PlayAnimation("attack");
            Invoke("DealSwordDamage", 0.2f);
        }
        else if (!isGrounded)
        {
            spriteAnimator.PlayAnimation("jump");
        }
        else if (Mathf.Abs(hAxis) > 0.1f)
        {
            spriteAnimator.PlayAnimation("run");
        }
        else
        {
            spriteAnimator.PlayAnimation("idle");
        }
    }

    public void TakeDamage(int amount)
    {
        _shieldLevel -= amount;

        if (spriteAnimator != null)
        {
            spriteAnimator.PlayAnimation("hit");
        }

        Debug.Log("Player took damage! Shield level: " + _shieldLevel);

        if (_shieldLevel <= 0)
        {
            if (spriteAnimator != null) spriteAnimator.PlayAnimation("death");
            Invoke("Die", 0.5f);
        }
    }

    void Die()
{
    Destroy(this.gameObject);

    string sceneName = SceneManager.GetActiveScene().name;

    switch (sceneName)
    {
        case "Scene_0":
            Main.HERO_DIED();
            break;
        case "Scene_Lvl2":
            MainLvl2.HERO_DIED();
            break;
        case "Scene_Lvl3":
            MainLvl3.HERO_DIED();
            break;
        case "Scene_Lvl4":
            MainLvl4.HERO_DIED();
            break;
        case "Scene_Lvl5":
            MainLvl5.HERO_DIED();
            break;
        default:
            Debug.LogWarning("Unrecognized scene name: " + sceneName);
            break;
    }
}


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void DealSwordDamage()
    {
        // Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(hitPoint.position, attackRange, enemyLayer);
         Collider2D[] hitEnemies = Physics2D.OverlapBoxAll((Vector2)hitBox.transform.position, hitBox.size, 0f, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Enemy enemy = enemyCollider.GetComponentInChildren<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

//     public void DealSwordDamage()
// {
//     // Calculate the center of the hitbox in world space
//     Vector2 center = (Vector2)hitBox.transform.position + hitBox.offset;
//     Vector2 size = hitBox.size;

//     // Get all enemies within the box area
//     Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(center, size, 0f, enemyLayer);

//     foreach (Collider2D enemyCollider in hitEnemies)
//     {
//         if (enemyCollider.CompareTag("Enemy"))
//         {
//             Enemy enemy = enemyCollider.GetComponentInChildren<Enemy>();
//             if (enemy != null)
//             {
//                 enemy.TakeDamage(attackDamage);
//             }
//         }
//     }
// }
}
