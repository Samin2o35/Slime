using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private RaycastHit2D groundHit;
    private float moveInput;
    private float jumpTimeCounter; 
    private bool isJumping;
    private bool isFalling;
    public bool isGrounded;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;

    [Header("Turn Check")]
    [SerializeField] private GameObject rLeg;
    [SerializeField] private GameObject lLeg;

    [Header("GroundCheck")]
    [SerializeField] private float extraHeight;
    [SerializeField] private LayerMask whatIsGround;

    [HideInInspector] public bool isFacingRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        StartDirectionCheck();
    }

    private void Update()
    {
        Move();
        Jump();
    }
    #region Movement
    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;

        if (moveInput > 0 || moveInput < 0)
        {
            anim.SetBool("isWalking", true);
            TurnCheck();
        }
        else 
        {
            anim.SetBool("isWalking", false);
        }

        rb.velocity = new Vector2 (moveInput * moveSpeed, rb.velocity.y);
    }
    #endregion

    #region TurnPlayer
    private void StartDirectionCheck()
    {
        if(rLeg.transform.position.x > lLeg.transform.position.x)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    private void TurnCheck()
    {
        if(UserInput.instance.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if(UserInput.instance.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation =Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion

    #region Jumping
    private void Jump()
    {
        //Button was pressed this frame
        if(UserInput.instance.controls.Jumping.Jump.WasPerformedThisFrame() && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //Button is being pressed
        else if(UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            if (jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        //Button was released this frame
        else if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
        }
        DrawGroundCheck();
    }

    private bool GroundCheck()
    {
        groundHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Debug Functions

    private void DrawGroundCheck()
    {
        Color rayColor;
        if(isGrounded)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(col.bounds.center + new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(col.bounds.center + new Vector3(col.bounds.extents.x, col.bounds.extents.y + extraHeight), Vector2.right * (col.bounds.extents.x * 2), rayColor);
    }

    #endregion
}
