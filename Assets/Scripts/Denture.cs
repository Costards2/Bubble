using UnityEngine;

public class Denture : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform pointRight;
    public Transform pointLeft;
    public AudioClip deathClip;

    private float right;
    private float left;
    private SpriteRenderer sprite;
    private Collider2D collider2D;
    private Rigidbody2D rb;

    private enum EnemyState
    {
        WalkingRight,
        WalkingLeft,
        Dead
    }

    private EnemyState currentState;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        right = pointRight.position.x;
        left = pointLeft.position.x;
        currentState = EnemyState.WalkingRight;

        sprite.flipX = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.WalkingRight:
                WalkRight();
                break;
            case EnemyState.WalkingLeft:
                WalkLeft();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    void WalkRight()
    {
        transform.Translate(Time.deltaTime * walkSpeed * Vector2.right);

        if (transform.position.x >= right)
        {
            currentState = EnemyState.WalkingLeft;
            sprite.flipX = false; // Flip the sprite to face left
        }
    }

    void WalkLeft()
    {
        transform.Translate(Time.deltaTime * walkSpeed * Vector2.left);

        if (transform.position.x <= left)
        {
            currentState = EnemyState.WalkingRight;
            sprite.flipX = true; // Flip the sprite to face right
        }
    }

    void Dead()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") && collision.gameObject.GetComponent<Bubble>().bubbleTrouble == false)
        {
            currentState = EnemyState.Dead;
            collider2D.enabled = false;

            AudioManager.instance.PlaySFX(deathClip);

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector3(0, 5, 0), ForceMode2D.Impulse);

            Destroy(gameObject, 3f);
        }
    }
}
