using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRoam : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float directionTimer;

    private Transform visual;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickNewDirection();

        // Find visual child (like "Sprite") to flip
        visual = transform.Find("Sprite");
        if (visual != null)
        {
            originalScale = visual.localScale;
        }
    }

    void Update()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0)
        {
            PickNewDirection();
        }

        rb.linearVelocity = moveDirection * moveSpeed;

        // Flip visual based on moveDirection.x
        if (visual != null && Mathf.Abs(moveDirection.x) > 0.01f)
        {
            visual.localScale = new Vector3(
                moveDirection.x > 0 ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }

    void PickNewDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        moveDirection = new Vector2(x, y).normalized;
        directionTimer = changeDirectionTime;
    }
}
