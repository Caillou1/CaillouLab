using System;
using UnityEngine;

public class CharacterAnimationComponent : CharacterComponent
{
    public Animator animatorController;

    private void LateUpdate()
    {
        UpdateAnimationParameters();
    }

    private void UpdateAnimationParameters()
    {
        animatorController.SetBool("Grounded", controlledCharacter.CharacterMovement.IsGrounded);
        animatorController.SetBool("Aiming", false);
        animatorController.SetBool("Crouching", false);
        animatorController.SetFloat("Forward", controlledCharacter.CharacterMovement.CurrentSpeed/controlledCharacter.CharacterMovement.MaxMovementSpeed);
        animatorController.SetFloat("Turn", controlledCharacter.CharacterMovement.TurnRate);
        animatorController.SetFloat("Right", 0);
    }
}
