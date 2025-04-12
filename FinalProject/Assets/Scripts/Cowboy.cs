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
    public Transform hitPoint; // The point where the sword hits
    public float attackRange = 1.5f;
    public float attackDamage = 5f;
    public LayerMask enemyLayer;

    [Header("Dynamic")] [Range(0, 4)]
    private float _shieldLevel = 1; // remember the underscore
    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    // Sprite flip support
    private Transform visual;
    private Vector3 originalScale;

    // Reference to SpriteAnimator script
    private SpriteAnimator spriteAnimator;

    //public delegate void WeaponFireDelegate();
    //public event WeaponFireDelegate fireEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Attempted to assign second Cowboy.S");
        }

        rb = GetComponent<Rigidbody2D>();

        visual = transform.Find("Sprite"); //change "Sprite" to your child object name
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

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        // float vAxis = Input.GetAxis("Vertical");

        Vector2 velocity = rb.linearVelocity;
        velocity.x = hAxis * speed;
        // velocity.z = 0f;
        rb.linearVelocity = velocity;

        // Flip character sprite left/right
        if (visual != null && Mathf.Abs(hAxis) > 0.01f)
        {
            spriteAnimator.FlipSprite(hAxis);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Vector3 pos = transform.position;
        // pos.x += hAxis * speed * Time.deltaTime;
        // pos.y += vAxis * speed * Time.deltaTime;
        // transform.position = pos;

        // Set animation state
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // make sure it's not the same triggering go as last time
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        //PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null)
        {
            _shieldLevel--;
            Destroy(go);
            Destroy(this.gameObject);
            Main.HERO_DIED();
        }
        // else if (pUp != null) {    // if shield hit a powerup absorb the powerup
        //     AbsorbPowerUp(pUp);
        // }
        else
        {
            Debug.LogWarning("Shield trigger hit by non-Enemy: " + go.name);
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
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, attackRange, enemyLayer);

    foreach (Collider2D enemyCollider in hitEnemies)
    {
        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
        }
    }
}
}
