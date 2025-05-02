//using System.Numerics;
using UnityEngine;

public class Sheriffs : Enemy
{

    [Header("Sheriff Private Fields")]
    [SerializeField] private Vector3 p0;
    
//private int direction = 1;
    public float jumpForce = 12f;
    public float jumpInterval = 1f;

    private Rigidbody2D rb;
    private float nextJumpTime;
    private bool isGrounded = false;

    void Start()
    {
        p0 = Vector3.zero;
        p0.x = bndCheck.camWidth + bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.TopScreen);

        bndCheck = GetComponent<BoundsCheck>();
        rb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + jumpInterval;
    }
     public override void Move()
     {

    //     // Vector3 pos = p0;
    //     // float t = Time.time - timeStart;
    //     // pos.y += Mathf.Sin(t * frequency) * amplitude; // up/down oscillation
    //     // transform.position = pos;

    //     Vector3 pos = transform.position;
    // pos.y += verticalSpeed * direction * Time.deltaTime;
    // transform.position = pos;

    // if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp))
    // {
    //     direction = -1;
    // }
    // else if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
    // {
    //     direction = 1;
    // }


    // if (Time.time >= nextJumpTime)
    // {
    //     Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //     if (rb != null)
    //     {
    //         rb.linearVelocity = new Vector2(0f, jumpForce);  // jump straight up
    //     }

    //     nextJumpTime = Time.time + jumpInterval;
    // }

    if (isGrounded && Time.time >= nextJumpTime)
    {
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.linearVelocity = new Vector2(0f, jumpForce);  // jump straight up
        nextJumpTime = Time.time + jumpInterval;
    }
     }

    void Update() {
    //if (Time.time >= nextJumpTime) {
        Move();
        //nextJumpTime = Time.time + jumpInterval;
   // }

    if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offLeft)) {
        Destroy(gameObject);
    }
}

// void Jump() {
//     Rigidbody2D rb = GetComponent<Rigidbody2D>();
//     if (rb != null) {
//         rb.linearVelocity = new Vector2(-speed, jumpForce); // move left while jumping up
//     }
// }

void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
