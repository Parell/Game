using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public bool canMove = true;
    public float moveDisabledTimer;
    [Space]
    [SerializeField] LayerMask terrainLayer;
    public Vector3 moveInput;
    [SerializeField] float moveSpeed = 4;
    [SerializeField] float airMoveSpeed = 2;
    [SerializeField] float moveAcceleration = 15;
    [SerializeField] float airMoveAcceleration = 8;
    [SerializeField] float turnAcceleration = 15;
    [SerializeField] Vector3 currentMovement;
    [Space]
    [SerializeField] float gravity = -10;
    [SerializeField] float groundedGravity = -1;
    [SerializeField] float fallMultiplier = 4;
    [SerializeField] bool grounded;
    [SerializeField] float rayToGroundLength = 3f;
    [SerializeField] Vector3 rayDir = Vector3.down;
    [Space]
    [SerializeField] float jumpPower = 0.25f;
    [SerializeField] float maxFallSpeed = -20;
    [SerializeField] bool isJumping = false;
    public bool isJumpPressed = false;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float coyoteTimeCounter;
    public float jumpBufferTime = 0.1f;
    public float jumpBufferTimeCounter;

    [Space]
    public bool isDashPressed = false;
    [SerializeField] float dashLength = 0.15f;
    [SerializeField] float dashSpeed = 2.5f;
    [SerializeField] float dashResetTime = 1;

    Vector3 dashMove;
    float dashing;
    float dashingTime;

    bool canDash = true;
    bool isDashing = false;
    bool dashReset = true;

    Vector3 goalVelocity;
    Vector3 velocity;

    Vector3 goalDirection;
    public Vector3 direction;

    CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        characterController.Move(currentMovement * Time.deltaTime);

        (bool rayHitGround, RaycastHit rayHit) = RaycastToGround();
        grounded = CheckIfGrounded(rayHitGround, rayHit);

        if (!canMove || moveDisabledTimer > 0)
        {
            isJumpPressed = false;
            isDashPressed = false;
            moveInput = Vector3.zero;
            moveDisabledTimer -= Time.deltaTime;
        }

        HandleMove();
        HandleGravity();
        HandleJump();
        HandleDash();
    }

    void HandleMove()
    {
        if (grounded)
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

    void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0f;

        if (grounded)
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

    void HandleJump()
    {
        characterController.slopeLimit = grounded ? 45 : 90;

        float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpPower);

        if (jumpBufferTimeCounter > 0 && coyoteTimeCounter > 0)
        {
            isJumping = true;
            jumpBufferTimeCounter = 0;

            currentMovement.y = jumpVelocity;
        }
        else if (!isJumpPressed && grounded)
        {
            isJumping = false;

            coyoteTimeCounter = 0;
        }
    }

    void HandleDash()
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

    bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
    {
        bool grounded;
        if (rayHitGround == true)
        {
            //grounded = rayHit.distance <= 1f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        return grounded;
    }

    (bool, RaycastHit) RaycastToGround()
    {
        RaycastHit rayHit;
        Ray rayToGround = new Ray(transform.position, rayDir);
        bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, rayToGroundLength, terrainLayer.value);
        Debug.DrawRay(transform.position, rayDir * rayToGroundLength, Color.blue);
        return (rayHitGround, rayHit);
    }
}
