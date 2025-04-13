using UnityEditor.Callbacks;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    static public Cowboy S { get; private set; }

    [Header("Inscribed")]
    public float speed = 10f;
    public float jumpForce = 10f;
    public float pitchMult = 30;
    [Header("Sword Attack Settings")]
    public Transform hitPoint;
    public float attackRange = 1.5f;
    public float attackDamage = 5f;
    public LayerMask enemyLayer;

    [Header("Dynamic")] [Range(0, 4)]
    private float _shieldLevel = 4; // Start with 4 hits allowed
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private Transform visual;
    private Vector3 originalScale;
    private SpriteAnimator spriteAnimator;

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
        Main.HERO_DIED();
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, attackRange, enemyLayer);

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
}
