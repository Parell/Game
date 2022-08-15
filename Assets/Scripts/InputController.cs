using UnityEngine;

public class InputController : MonoBehaviour
{
    CharacterMover characterMover;

    void Awake()
    {
        characterMover = GetComponent<CharacterMover>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 _moveInput;

        _moveInput = new Vector3(horizontalInput, 0, verticalInput);
        _moveInput = Vector3.ClampMagnitude(_moveInput, 1f);

        characterMover.moveInput = _moveInput;

        if (Input.GetButton("Jump"))
        {
            characterMover.isJumpPressed = true;
            characterMover.jumpBufferTimeCounter = characterMover.jumpBufferTime;
        }
        else
        {
            characterMover.jumpBufferTimeCounter -= Time.deltaTime;
        }

        characterMover.isDashPressed = Input.GetButtonDown("Fire2");
    }
}
