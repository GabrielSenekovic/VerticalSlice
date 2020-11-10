using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerControls controls;

    float direction = 0;
    [SerializeField]float speed;
    Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    Rigidbody2D body;

    public int amountOfJumps = 0;
    [SerializeField]int amountOfJumps_Max;
    public bool grounded = true;
    public bool jumping = false;
    public float jumpSpeed;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField]Transform groundCheck;
    const float groundedRadius = .05f;
    [SerializeField]Transform ceilingCheck;
    const float ceilingRadius = .05f;

    int jump_Limit = 30;
    int jump_Timer = 0;
    [SerializeField]float jumpForce;

    private void Start() 
    {
        jump_Timer = jump_Limit;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(body.velocity.y <= 0 && jump_Timer > 10 && !grounded) //if falling
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Ground();
                }
            }
        }
    }

    private void FixedUpdate() 
    {
        Vector3 targetVelocity = new Vector2(direction * speed, body.velocity.y);
		body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref velocity, movementSmoothing);
        if(jumping)
        {
            jump_Timer++;
        }
    }
    private void OnMove(InputValue value)
    {
        direction = value.Get<float>();
    }
    private void OnAttack()
    {
        Debug.Log("Attack");
    }

    private void Ground()
    {
        Debug.Log("Ground");
        grounded = true;
        amountOfJumps = 0;
        jumping = false;
        jump_Timer = jump_Limit;
        body.velocity = new Vector2(body.velocity.x, 0);
    }
    private void OnJump()
    {
        if(jump_Timer < jump_Limit) {return;}
        if(grounded || amountOfJumps < amountOfJumps_Max)
        {
            Debug.Log("Jump");
            jumping = true; grounded = false;
            if(body.velocity.y < 0)
            {
                body.velocity = new Vector2(body.velocity.x, 0);
            }
			body.AddForce(new Vector2(0f, jumpForce));
            amountOfJumps++;
            jump_Timer = 0;
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }
    void OnDisable()
    {
        controls.Disable();
    }
}
