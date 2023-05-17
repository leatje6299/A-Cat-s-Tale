using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private TailStateManager tailState;
    private CheckpointManager checkpointManager;

    [Header("Inputs")]
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;

    [Header("Movement")]
    [SerializeField] private float swingSpeed;
    [SerializeField] private float stompForce;
    [SerializeField] private float jumpCD;
    [SerializeField] private float airMult;
    public List<float> gravityAndSpeed;
    private float moveSpeed;
    Vector3 moveDirection;
    public Vector2 movement;
    public bool isSwinging;
    private bool canJump;
    private bool canMove;
    private bool isFalling;
    private float fallingTime = 1f;

    [Header("Stomp")]
    public bool canStomp;
    public string objectCollided;
    public bool hasStomped;

    [Header("Grounded")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundDrag;
    private bool isGrounded;
    private bool isUnderwater;
    private Vector3 waterLevel;

    [Header("Player")]
    [SerializeField] private CapsuleCollider playerCollider;
    private float playerHeight = 1.9f;
    public Transform orientation;
    public Rigidbody rb;
    [SerializeField] private GameObject deathScreen;

    [Header("Animation")]
    public Animator animator;

    [Header("State")]
    private MovementState state;
    private enum MovementState
    {
        walking,
        swinging,
        idle,
        falling,
        scared
    }
    public bool isScared;

    private void Start()
    {
        tailState = GetComponent<TailStateManager>();
        checkpointManager = GetComponent<CheckpointManager>();
        state = MovementState.idle;
        canJump = true;
        canMove = true;
        isScared = false;
        objectCollided = null;
        isUnderwater = false;
    }

    private void Update()
    {
        gravityAndSpeed = tailState.GetPlayerSpeedAndGravity();
        moveSpeed = gravityAndSpeed[1];
        GroundChecks();
        UnderwaterCheck();
        MaxSpeed();
        Inputs();
        GetState();
        StateAnimation();

        animator.SetBool("isSwinging", isSwinging);
        animator.SetBool("isFalling", isFalling);
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // Get the current velocity of the Rigidbody
        Vector3 velocity = rb.velocity;

        // Clamp the Y velocity using Mathf.Clamp
        velocity.y = Mathf.Clamp(velocity.y, -20f, 30f);

        // Assign the clamped velocity back to the Rigidbody
        rb.velocity = velocity;
    }
    
    //CHECKS
    private void GroundChecks()
    {
        //ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        rb.drag = isGrounded ? groundDrag : 2f;
        //rb.drag = 2f;

        RaycastHit hit; 
        if(Physics.Raycast(transform.position, Vector3.down, out hit, ground))
        {
            float distFromGround = hit.distance;
            if (distFromGround > 1.5 * playerHeight)
            {
                canStomp = true;
            }
            else canStomp = false;
        }

        isFalling = false;
    }

    private void UnderwaterCheck()
    {
        if(isUnderwater)
        {
            if(playerCollider != null)
            {
                Vector3 colliderCentre = playerCollider.bounds.center;
                if(colliderCentre.y < waterLevel.y)
                {
                    animator.SetTrigger("isDead");
                    isUnderwater = false;
                    deathScreen.GetComponent<Animator>().SetBool("deathScreen", true);
                    Invoke("PlayerDeath", 1.25f);
                }
            }
        }
    }   
    
    //INPUTS
    private void Inputs()
    {
        movement = movementControl.action.ReadValue<Vector2>();

        if (jumpControl.action.triggered && canJump && isGrounded)
        {
            canJump = false;
            JumpInput();
        }

        if(jumpControl.action.triggered && canStomp && !isGrounded && tailState.currentState == tailState.TailStrong)
        {
            rb.AddForce(transform.up * stompForce, ForceMode.Impulse);
            hasStomped = true;
        }
    }

    //STATE
    private void GetState()
    {
        playerCollider.enabled = (state == MovementState.swinging) ? false : true;

        if (isGrounded && isScared)
        {
            state = MovementState.scared;
        }
        else if (isGrounded && rb.velocity == Vector3.zero)
        {
            state = MovementState.idle;
        }
        else if(!isGrounded && !isSwinging)
        {
            state = MovementState.falling;
        }
        else if (isGrounded && !isSwinging)
        {
            state = MovementState.walking;
        }
        else if (isSwinging)
        {
            state = MovementState.swinging;
            moveSpeed = swingSpeed;
        }
    }

    private void StateAnimation()
    {
        switch (state)
        {
            case MovementState.swinging:
                animator.SetBool("isFalling", false);
                break;
            case MovementState.walking:
                if (Mathf.Abs(rb.velocity.x) > 0.01f || Mathf.Abs(rb.velocity.z) > 0.01f)
                {
                    animator.SetBool("isWalking", true);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
                break;
            case MovementState.idle:
                animator.SetBool("isWalking", false);
                break;
            case MovementState.scared:
                animator.SetBool("isScared", true);
                break;
            case MovementState.falling:
                fallingTime = 1f;
                isFalling = true;
                break;
        }
    }

    private void PlayerDeath()
    {
        checkpointManager.LastCheckPoint();
        deathScreen.GetComponent<Animator>().SetBool("deathScreen", false);
    }

    //MOVEMENT FUNCTIONS
    private void MovePlayer()
    {
        if (isSwinging) return;
        if(isFalling)
        {
            rb.AddForce(new Vector3(0, GetFallingGravity(), 0));
        }
        moveDirection = orientation.forward * movement.y + orientation.right * movement.x;

        if(canMove)
        {
            if (isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMult, ForceMode.Force);
            }
        }
    }

    private void MaxSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void JumpInput()
    {
        canMove = false;
        animator.SetBool("isJumping", true);
        Invoke("Jump", 0.3f);
    }

    private void Jump()
    {
        canMove = true;
        List<float> gravityAndSpeed = tailState.GetPlayerSpeedAndGravity();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * gravityAndSpeed[2], ForceMode.Impulse);
        fallingTime = 1f;
        rb.AddForce(new Vector3(0, GetFallingGravity(), 0));
        canJump = true;
    }

    private float GetFallingGravity()
    {
        fallingTime += Time.deltaTime;
        return gravityAndSpeed[0] * fallingTime;
    }

    //COLLISIONS
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("isJumping", false);
        if (collision.gameObject.tag == "stompSpot")
        {
            if(hasStomped)
            {
                objectCollided = collision.gameObject.name;
                collision.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            waterLevel = other.transform.position;
            isUnderwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = false;
        }
    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }
}
