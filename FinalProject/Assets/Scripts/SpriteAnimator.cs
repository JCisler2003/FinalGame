using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;

    [Header("Animations")]
    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public Sprite[] attackSprites;
    public Sprite[] hitSprites;
    public Sprite[] deathSprites;

    public float frameRate = 0.1f;

    private float timer;
    private int frameIndex;

    private enum AnimState { Idle, Run, Jump, Attack, Hit, Death }
    private AnimState currentState = AnimState.Idle;

    private bool isLocked = false;
    private float lockTimer = 0f;

    void Update()
    {
        if (isLocked)
        {
            lockTimer -= Time.deltaTime;
            if (lockTimer <= 0f)
            {
                isLocked = false;
                if (currentState == AnimState.Attack || currentState == AnimState.Hit)
                {
                    SetAnimation(AnimState.Idle);
                }
            }
        }

        // Advance frames
        timer += Time.deltaTime;
        Sprite[] currentSprites = GetCurrentSprites();

        if (currentSprites.Length > 0 && timer >= frameRate)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[frameIndex];
        }
    }

    public void PlayAnimation(string anim)
    {
        if (isLocked && anim != "attack" && anim != "hit" && anim != "death") return;

        if (anim == "attack" && currentState != AnimState.Attack)
        {
            SetAnimation(AnimState.Attack);
            isLocked = true;
            lockTimer = attackSprites.Length * frameRate;
            return;
        }

        if (anim == "hit" && currentState != AnimState.Hit)
        {
            SetAnimation(AnimState.Hit);
            isLocked = true;
            lockTimer = hitSprites.Length * frameRate;
            return;
        }

        if (anim == "death" && currentState != AnimState.Death)
        {
            SetAnimation(AnimState.Death);
            isLocked = true;
            lockTimer = deathSprites.Length * frameRate;
            return;
        }

        if (anim == "idle" && currentState != AnimState.Idle)
        {
            SetAnimation(AnimState.Idle);
        }
        else if (anim == "run" && currentState != AnimState.Run)
        {
            SetAnimation(AnimState.Run);
        }
        else if (anim == "jump" && currentState != AnimState.Jump)
        {
            SetAnimation(AnimState.Jump);
        }
    }

    void SetAnimation(AnimState state)
    {
        currentState = state;
        frameIndex = 0;
        timer = 0f;

        Sprite[] sprites = GetCurrentSprites();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    Sprite[] GetCurrentSprites()
    {
        switch (currentState)
        {
            case AnimState.Run: return runSprites;
            case AnimState.Jump: return jumpSprites;
            case AnimState.Attack: return attackSprites;
            case AnimState.Hit: return hitSprites;
            case AnimState.Death: return deathSprites;
            default: return idleSprites;
        }
    }

    public void FlipSprite(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = direction > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
