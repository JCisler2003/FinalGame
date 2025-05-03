using UnityEngine;

public class Skeleton : Enemy
{

    private float direction = -1f;
    private bool isDying = false;
    private SpriteAnimator spriteAnimator;
    void Start()
    {
        
    }

    void Update()
    {
         if (!isDying)
        {
            Move();

           
        }
    }

    public override void Move()
    {
           Vector3 tempPos = pos;
    tempPos.x += direction * speed * Time.deltaTime;
    pos = tempPos;

    
    if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offLeft) || bndCheck.LocIs(BoundsCheck.eScreenLocs.offRight))
    {
        direction *= -1;
        spriteAnimator.FlipSprite(direction);
    }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cowboy cowboy = other.GetComponent<Cowboy>();
            if (cowboy != null)
            {
                Debug.Log("I got hit");
                cowboy.TakeDamage(1);
            }
        }
    }
}
