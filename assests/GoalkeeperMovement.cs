
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoalkeeperMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Diving")]
    public float diveSpeed = 5f;
    public float diveUpwardSpeed = 0f;
    public float diveDuration = 0.5f;
    public float diveCooldown = 1f;

    [Header("Dive Animation Timing")]
    public float diveStartDelay = 0.25f;

    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity;
    private float diveVelocity;
    private float diveTimer;
    private float diveDelayTimer;
    private float nextDiveTime;

    private bool isDiving;
    private bool isPreparingDive;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.applyRootMotion = false;
            StopMovementAnimations();
        }
        else
        {
            Debug.LogWarning("Goalkeeper Animator not found!");
        }
    }

    void Update()
    {
        bool grounded = controller.isGrounded;

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // SPACEBAR: Jump and play catch animation
        if (Input.GetKeyDown(KeyCode.Space)
            && grounded
            && !isDiving
            && !isPreparingDive)
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }

        // Q/E: Start diving
        if (grounded
            && !isDiving
            && !isPreparingDive
            && Time.time >= nextDiveTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartDive(1f, "Left_Dive");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartDive(-1f, "Right_Dive");
            }
        }

        Vector3 horizontalMovement = Vector3.zero;

        if (isPreparingDive)
        {
            diveDelayTimer -= Time.deltaTime;

            if (diveDelayTimer <= 0f)
            {
                isPreparingDive = false;
                isDiving = true;
                verticalVelocity = diveUpwardSpeed;
            }
        }

        if (isDiving)
        {
            horizontalMovement =
                Vector3.right * diveVelocity;

            diveTimer -= Time.deltaTime;

            if (diveTimer <= 0f)
            {
                isDiving = false;
                diveVelocity = 0f;
            }
        }
        else if (!isPreparingDive)
        {
            float horizontal =
                Input.GetAxisRaw("Horizontal");

            float vertical =
                Input.GetAxisRaw("Vertical");

            horizontalMovement = new Vector3(
                -horizontal,
                0f,
                -vertical
            ).normalized * moveSpeed;

            UpdateMovementAnimation(horizontal, vertical);
        }

        if (isDiving || isPreparingDive)
        {
            StopMovementAnimations();
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement =
            horizontalMovement +
            Vector3.up * verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }

    void UpdateMovementAnimation(float horizontal, float vertical)
    {
        if (animator == null)
            return;

        bool moveLeft = false;
        bool moveRight = false;
        bool moveForward = false;
        bool moveBackwards = false;

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            if (horizontal < 0f)
                moveLeft = true;
            else
                moveRight = true;
        }
        else if (Mathf.Abs(vertical) > 0.1f)
        {
            if (vertical > 0f)
                moveForward = true;
            else
                moveBackwards = true;
        }

        animator.SetBool("Move_Left", moveLeft);
        animator.SetBool("Move_Right", moveRight);
        animator.SetBool("Move_Forward", moveForward);
        animator.SetBool("Move_Backwards", moveBackwards);
    }

    void StopMovementAnimations()
    {
        if (animator == null)
            return;

        animator.SetBool("Move_Left", false);
        animator.SetBool("Move_Right", false);
        animator.SetBool("Move_Forward", false);
        animator.SetBool("Move_Backwards", false);
    }

    void StartDive(float direction, string animationTrigger)
    {
        isPreparingDive = true;
        isDiving = false;

        diveDelayTimer = diveStartDelay;
        diveTimer = diveDuration;
        diveVelocity = direction * diveSpeed;

        nextDiveTime = Time.time + diveCooldown;

        StopMovementAnimations();

        if (animator != null)
        {
            animator.ResetTrigger("Left_Dive");
            animator.ResetTrigger("Right_Dive");
            animator.SetTrigger(animationTrigger);
        }
    }
}
