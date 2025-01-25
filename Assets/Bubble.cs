using UnityEngine;
using UnityEngine.EventSystems;

public class Bubble : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float jumpYVelocity = 15f;
    [SerializeField] float moveVelocity = 10f;

    [Header("Input Settings")]
    [SerializeField] float horizontalInput;
    [SerializeField] bool jumpInput;

    bool isGrounded;

    [Header("Componets")]
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    enum State { Idle, Run, Jump, Fall, Attack}

    State state = State.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKey(KeyCode.Space);

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
        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Run: RunState(); break;
            case State.Jump: JumpState(); break;
            case State.Fall: FallState(); break;
            case State.Attack: AttackState(); break;
        }
    }

    void IdleState()
    {
        // actions
        //animator.Play("Idle");

        // transitions
        if (isGrounded)
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
        //animator.Play("Run");
        rb.linearVelocity = new Vector2(moveVelocity, rb.linearVelocityY) * horizontalInput;

        if (isGrounded)
        {
            if (jumpInput)
            {
                state = State.Jump;
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

    void JumpState()
    {
        // actions
        //animator.Play("Jump");

        rb.linearVelocity = (moveVelocity * horizontalInput * Vector2.right) + (jumpYVelocity * Vector2.up);

        state = State.Fall;
    }

    void AttackState()
    {

    }

    void FallState()
    {
        //Fall Harder
        if(Input.GetAxis("Vertical") < 0)
        {
            rb.gravityScale = 2.5f;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        rb.linearVelocity = (rb.linearVelocity.y * Vector2.up) + (moveVelocity * horizontalInput * Vector2.right);

        if (isGrounded)
        {
            rb.gravityScale = 1f;

            if (horizontalInput != 0f && rb.linearVelocity.y == 0f)// Remove this if
            {
                state = State.Run;
            }
            else
            {
                state = State.Idle;
            }
        }
        else if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            state = State.Jump;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            coyoteTimeCounter -= Time.deltaTime;
            isGrounded = false;
        }
    }

}
