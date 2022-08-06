using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public UnityEvent<Vector3> OnMoveInput;
    public UnityEvent OnJump;
    public UnityEvent OnAttack;
    public UnityEvent OnDash;

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveInput;

        moveInput = new Vector3(horizontalInput, 0, verticalInput);
        moveInput = Vector3.ClampMagnitude(moveInput, 1f);

        OnMoveInput?.Invoke(moveInput);

        if (Input.GetButtonDown("Jump"))
        {
            PerformJump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            PerformDash();
        }
    }

    private void PerformJump()
    {
        OnJump?.Invoke();
    }

    private void PerformAttack()
    {
        OnAttack?.Invoke();
    }

    private void PerformDash()
    {
        OnDash?.Invoke();
    }
}
