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
        animatorController.SetBool("Aiming", controlledCharacter.CharacterPickup.HasObjectInHands);
        animatorController.SetBool("Crouching", false);
        animatorController.SetFloat("Forward", controlledCharacter.CharacterMovement.CurrentSpeed/controlledCharacter.CharacterMovement.CurrentMaxMovementSpeed * (controlledCharacter.CharacterPickup.HasObjectInHands ? Vector3.Dot(controlledCharacter.CharacterMovement.Forward, controlledCharacter.CharacterController.LeftStickDirection) : 1));
        animatorController.SetFloat("Turn", controlledCharacter.CharacterPickup.HasObjectInHands ? 0 : controlledCharacter.CharacterMovement.TurnRate);
        animatorController.SetFloat("Right", controlledCharacter.CharacterPickup.HasObjectInHands ? controlledCharacter.CharacterMovement.CurrentSpeed/controlledCharacter.CharacterMovement.CurrentMaxMovementSpeed * Vector3.Dot(controlledCharacter.CharacterMovement.Right, controlledCharacter.CharacterController.LeftStickDirection) : 0);
        animatorController.SetLayerWeight(1, controlledCharacter.CharacterPickup.HasObjectInHands ? 1 : 0);
    }
}
