using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float health = 10;
    public int score = 100;

    protected BoundsCheck bndCheck;
    private SpriteAnimator spriteAnimator;
    private bool isDying = false;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        spriteAnimator = GetComponentInChildren<SpriteAnimator>();
    }

    public Vector3 pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    void Update()
    {
        if (!isDying)
        {
            Move();

            if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offLeft))
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x -= speed * Time.deltaTime;
        pos = tempPos;
    }

    public void TakeDamage(float amount)
    {
        if (isDying) return;

        health -= amount;

        if (spriteAnimator != null)
        {
            spriteAnimator.PlayAnimation("hit");
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDying) return;
        isDying = true;

        if (spriteAnimator != null)
        {
            spriteAnimator.PlayAnimation("death");
        }

        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cowboy cowboy = other.GetComponent<Cowboy>();
            if (cowboy != null)
            {
                cowboy.TakeDamage(1);
            }
        }
    }
}