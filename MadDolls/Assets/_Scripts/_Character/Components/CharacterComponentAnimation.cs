using UnityEngine;

namespace Gameplay.Character
{
    [System.Serializable]
    public class CharacterComponentAnimation : ACharacterComponent
    {
        public Animator animatorController;

        private float targetAimWeight;
        private float currentAimWeight;
        private float aimVelocity;

        public override void LateUpdateComponent(float deltaTime)
        {
            UpdateAnimationParameters();
        }

        private void UpdateAnimationParameters()
        {
            animatorController.SetBool("Grounded", controlledCharacter.CharacterMovement.IsGrounded);
            animatorController.SetBool("Aiming", controlledCharacter.CharacterMovement.IsAiming);
            animatorController.SetBool("Crouching", false);
            animatorController.SetFloat("Forward", controlledCharacter.CharacterMovement.CurrentSpeed / controlledCharacter.CharacterMovement.CurrentMaxMovementSpeed * (controlledCharacter.CharacterMovement.IsAiming ? (controlledCharacter.CharacterController.LeftStickDirection.magnitude > .01f ? Vector3.Dot(controlledCharacter.CharacterMovement.Forward, controlledCharacter.CharacterController.LeftStickDirection) : 1) : 1));
            animatorController.SetFloat("Turn", controlledCharacter.CharacterMovement.IsAiming ? 0 : controlledCharacter.CharacterMovement.TurnRate);
            animatorController.SetFloat("Right", controlledCharacter.CharacterMovement.IsAiming ? controlledCharacter.CharacterMovement.CurrentSpeed / controlledCharacter.CharacterMovement.CurrentMaxMovementSpeed * Vector3.Dot(controlledCharacter.CharacterMovement.Right, controlledCharacter.CharacterController.LeftStickDirection) : 0);

            targetAimWeight = controlledCharacter.CharacterMovement.IsAiming ? 1 : 0;
            currentAimWeight = Mathf.SmoothDamp(currentAimWeight, targetAimWeight, ref aimVelocity, .25f);
            animatorController.SetLayerWeight(1, currentAimWeight);
        }
    }
}