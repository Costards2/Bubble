using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Bubble : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float jumpYVelocity = 15f;
    [SerializeField] float moveVelocity = 10f;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask groundLayer;

    [Header("Input Settings")]
    [SerializeField] float horizontalInput;
    [SerializeField] bool jumpInput;

    public bool bubbleTrouble = false;
    bool death;

    [Header("Componets")]
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D collider2D;
    public SpriteRenderer sprite;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    enum State { Idle, Run, Jump, Fall, Bubble}

    State state = State.Idle;

    void Start()
    {
        death = false;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (death) return;

        Flip();

        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.E) && bubbleTrouble == false)
        {
            bubbleTrouble = true;
            state = State.Bubble;
        }
        else if (Input.GetKeyDown(KeyCode.E) && bubbleTrouble == true)
        {
            bubbleTrouble = false;
            boxSize = new Vector2(0.5f, 0.1f);
            castDistance = 1.09f;
            state = State.Idle;
        }

        if (jumpInput)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (death) return;
       
        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Run: RunState(); break;
            case State.Jump: JumpState(); break;
            case State.Fall: FallState(); break;
            case State.Bubble: BubbleTrouble(); break;
        }
    }

    void IdleState()
    {
        animator.SetInteger("IDAnim", 0);


        if (bubbleTrouble)
        {
            state = State.Bubble;
        }

        if (IsGrounded())
        {
            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (horizontalInput != 0f)
            {
                state = State.Run;
            }
        }
        else
        {
            state = State.Fall;
        }
    }

    void RunState()
    {
        // actions
        animator.SetInteger("IDAnim", 1);

        rb.linearVelocity = new Vector2(moveVelocity * horizontalInput, rb.linearVelocityY);
       
        if (IsGrounded()) 
        {
            rb.AddForceY(2f, ForceMode2D.Impulse);
        }

        if (IsGrounded())
        {
            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (horizontalInput == 0f && rb.linearVelocityX == 0)
            {
                state = State.Idle;
            }
        }
        else
        {
           state = State.Fall;
        }
    }

    void JumpState()
    {
        rb.linearVelocity = (moveVelocity * horizontalInput * Vector2.right) + (jumpYVelocity * Vector2.up);

        state = State.Fall;
    }


    void FallState()
    {
        //Fall Harder
        if (Input.GetAxis("Vertical") < 0)
        {
            rb.gravityScale = 2.5f;
        }
        else
        {
            if (bubbleTrouble)
            {
                rb.gravityScale = 0.5f;
            }
            else
            {
                rb.gravityScale = 1f;
            }
        }

        rb.linearVelocity = (rb.linearVelocity.y * Vector2.up) + (moveVelocity * horizontalInput * Vector2.right);


        if (jumpInput && bubbleTrouble)
        {
            state = State.Jump;
        }

        if (IsGrounded())
        {
            if (!bubbleTrouble)
            {
                rb.gravityScale = 1f;

                if (horizontalInput != 0f)
                {
                    state = State.Run;
                }
                else if (horizontalInput == 0f && rb.linearVelocityX == 0)
                {
                    state = State.Idle;
                }
            }
            else if (bubbleTrouble)
            {
                state = State.Bubble;
            }
        }
        else if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            state = State.Jump;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    // Make the player inflates 
    void BubbleTrouble()
    {
        animator.SetInteger("IDAnim", 2);

        boxSize = new Vector2(0.95f, 0.1f);
        castDistance = 1.75f;

        rb.gravityScale = 0.5f;

        rb.linearVelocity = new Vector2(moveVelocity * horizontalInput, rb.linearVelocityY);

        if (IsGrounded())
        {
            rb.AddForceY(2f, ForceMode2D.Impulse);
        }

        if (jumpInput)
        {
            rb.linearVelocity = (moveVelocity * horizontalInput * Vector2.right) + (jumpYVelocity * Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bubbleTrouble = false;

            boxSize = new Vector2(0.5f, 0.1f);
            castDistance = 1.09f;

            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (horizontalInput != 0f)
            {
                state = State.Run;
            }
            else if (horizontalInput == 0f)
            {
                state = State.Idle;
            }
        }
        else
        {
            state = State.Fall;
        }
    }

    void Flip()
    {
        if (horizontalInput > 0f)
        {
            sprite.flipX = false;
        }
        else if (horizontalInput < 0f)
        {
            sprite.flipX = true;
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            coyoteTimeCounter = coyoteTime;
            return true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * castDistance, boxSize);
    }

    public void ActivateBubbleTrouble()
    {
        state = State.Bubble;
    }

    public void LittleUp()
    {
        rb.AddForceY(15f, ForceMode2D.Impulse);
    }

    public void Death()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !bubbleTrouble)
        {
            LittleUp();
        }
        else if(collision.tag == "Enemy")
        {
            // Make a method for this 
            death = true;
            rb.simulated = false;
            collider2D.enabled = false;
            animator.SetTrigger("Death");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            // Make a method for this 
            death = true;
            rb.simulated = false;
            collider2D.enabled = false;
            animator.SetTrigger("Death");
        }
    }
}
