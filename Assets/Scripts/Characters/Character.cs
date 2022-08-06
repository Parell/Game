using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimator characterAnimator;
    private CharacterMover characterMover;
    private WeaponController weaponController;

    private Vector3 moveInput;

    public Vector3 MoveInput { get => moveInput; set => moveInput = value; }

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterMover = GetComponent<CharacterMover>();
        weaponController = GetComponentInChildren<WeaponController>();
    }

    private void Update()
    {
        characterMover.moveInput = MoveInput;
        AnimateCharacter();
    }

    public void PerformJump()
    {
        characterMover.isJumpPressed = true;
    }

    public void PerformAttack()
    {
        weaponController.Attack();
    }

    public void PerformDash()
    {
        characterMover.isDashPressed = true;
    }

    private void AnimateCharacter()
    {
    }
}
