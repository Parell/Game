using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    private CharacterController characterController;

    public float moveDisabledTimer = 0;

    [Space]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float airMoveSpeed = 2f;
    [SerializeField] private float moveAcceleration = 15f;
    [SerializeField] private float airMoveAcceleration = 8f;
    [SerializeField] private float turnAcceleration = 15f;
    [SerializeField] private Vector3 currentMovement;

    [Space]
    [SerializeField] private float gravity = -10f;
    [SerializeField] private float groundedGravity = -1f;
    [SerializeField] private float fallMultiplier = 4f;

    [Space]
    [SerializeField] private float jumpHeight = 0.4f;
    [SerializeField] private float maxFallSpeed = -20f;
    [SerializeField] private bool isJumping = false;
    public bool isJumpPressed = false;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferTimeCounter;

    [Space]
    public bool isDashPressed = false;
    [SerializeField] private float dashLength = 0.15f;
    [SerializeField] private float dashSpeed = 2.5f;
    [SerializeField] private float dashResetTime = 1f;

    Vector3 dashMove;
    float dashing = 0f;
    float dashingTime = 0f;

    bool canDash = true;
    bool isDashing = false;
    bool dashReset = true;

    Vector3 goalVelocity;
    Vector3 velocity;

    Vector3 goalDirection;
    Vector3 direction;

    public Vector3 moveInput { get; set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        characterController.Move(currentMovement * Time.deltaTime);

        HandleMove();
        HandleGravity();
        HandleJump();
        HandleDash();
    }

    private void HandleMove()
    {

        if (characterController.isGrounded)
        {
            goalVelocity = moveInput * moveSpeed;
            velocity = Vector3.MoveTowards(velocity, goalVelocity, Time.deltaTime * moveAcceleration);
        }
        else
        {
            goalVelocity = moveInput * airMoveSpeed;
            velocity = Vector3.MoveTowards(velocity, goalVelocity, Time.deltaTime * airMoveAcceleration);
        }

        Transform camera = Camera.main.transform;

        goalDirection = camera.forward * velocity.z + camera.right * velocity.x;

        float goalDirectionLength = goalDirection.magnitude;
        goalDirection.y = 0;
        goalDirection = goalDirection.normalized * goalDirectionLength;

        if (goalDirection != Vector3.zero)
        {
            direction = Vector3.Slerp(direction, goalDirection, Time.deltaTime * turnAcceleration);

            transform.rotation = Quaternion.LookRotation(direction);
            characterController.Move(direction * Time.deltaTime);

            //animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    private void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0f;

        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;

            coyoteTimeCounter = coyoteTime;
        }
        else if (isFalling)
        {
            float lastYVelocity = currentMovement.y;
            float nextYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float newYVelocity = Mathf.Max((lastYVelocity + nextYVelocity) / 2, maxFallSpeed);
            currentMovement.y = newYVelocity;

            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            float lastYVelocity = currentMovement.y;
            float nextYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float newYVelocity = (lastYVelocity + nextYVelocity) / 2;
            currentMovement.y = newYVelocity;

            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (isJumpPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
            isJumpPressed = false;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        characterController.slopeLimit = characterController.isGrounded ? 45 : 90f;

        float jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);

        if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f)
        {
            isJumping = true;
            jumpBufferTimeCounter = 0f;

            currentMovement.y = jumpVelocity;
        }
        else if (!isJumpPressed && characterController.isGrounded)
        {
            isJumping = false;

            coyoteTimeCounter = 0f;
        }
    }

    private void HandleDash()
    {
        if (isDashPressed && dashing < dashLength && dashingTime < dashResetTime && dashReset == true && canDash == true)
        {
            dashMove = goalDirection;
            canDash = false;
            dashReset = false;
            isDashing = true;
        }

        if (isDashing && dashing < dashLength)
        {
            characterController.Move(dashMove * dashSpeed * Time.deltaTime);
            dashing += Time.deltaTime;
        }

        if (dashing >= dashLength)
        {
            isDashing = false;
        }

        if (dashReset == false)
        {
            dashingTime += Time.deltaTime;
        }

        if (canDash == false && dashing >= dashLength)
        {
            canDash = true;
            dashing = 0f;
        }

        if (dashingTime >= dashResetTime && dashReset == false)
        {
            dashReset = true;
            dashingTime = 0f;
        }

        isDashPressed = false;
    }
}
