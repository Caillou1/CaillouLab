using UnityEngine;

namespace Gameplay.Character
{
    [System.Serializable]
    public class CharacterComponentMovement : ACharacterComponent
    {
        [Header("Movement")]
        public float MaxMovementSpeed = 5f;
        public float MaxAimingMovementSpeed = 3f;
        public float Acceleration = 5f;
        public float Deceleration = 5f;

        [Header("Rotation")]
        public float RotationSpeed = 5f;
        public float TurnRateSpeed = 2f;

        [Header("Layers")]
        public LayerMask GroundLayer;

        private Vector3 velocity;
        public float CurrentMaxMovementSpeed
        {
            get;
            private set;
        }
        public float CurrentSpeed { get; private set; }

        public Vector3 Forward
        {
            get
            {
                if (controlledCharacter.CharacterMovement.IsAiming)
                {
                    return controlledCharacter.CharacterIK.RealAimDirection;
                }
                else
                {
                    return (velocity.magnitude > .01f) ? velocity.normalized : controlledCharacter.CharacterTransform.forward;
                }
            }
        }

        public Vector3 Right
        {
            get
            {
                return controlledCharacter.CharacterTransform.right;
            }
        }

        public float TurnRate
        {
            get
            {
                var forward = new Vector3(Forward.z, 0, -Forward.x);
                var dir = controlledCharacter.CharacterController.LeftStickDirection;
                var dot = Vector3.Dot(forward, dir);
                float rate = (dot > 0.25f || dot < -0.25f) ? Mathf.Sign(dot) : 0;
                currentTurnRate = Mathf.Lerp(currentTurnRate, rate, Time.deltaTime * TurnRateSpeed);
                return currentTurnRate;
            }
        }

        public bool IsAiming
        {
            get
            {
                return controlledCharacter.CharacterIK.IsAimingEnabled;
            }
        }

        private float currentTurnRate { get; set; }

        public bool IsGrounded
        {
            get
            {
                return Physics.Raycast(controlledCharacter.CharacterTransform.position + Vector3.up, Vector3.down, 1.25f, GroundLayer);
            }
        }

        public override void UpdateComponent(float deltaTime)
        {
            UpdateSpeed(deltaTime);
            RotateBody();
        }

        public override void FixedUpdateComponent(float fixedDeltaTime)
        {
            MoveBody(fixedDeltaTime);
        }

        private void UpdateSpeed(float dt)
        {
            CurrentMaxMovementSpeed = controlledCharacter.CharacterMovement.IsAiming ? MaxAimingMovementSpeed : MaxMovementSpeed;
            var input = controlledCharacter.CharacterController.LeftStickDirection;
            if (input.magnitude > 0.05f)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed + Acceleration * dt, 0, CurrentMaxMovementSpeed);
                var forwardVector = velocity.magnitude > 0.05f ? velocity.normalized : controlledCharacter.CharacterTransform.forward;
                if (controlledCharacter.CharacterMovement.IsAiming)
                {
                    velocity = Vector3.Lerp(forwardVector, input, RotationSpeed * 5 * dt) * CurrentSpeed;
                }
                else
                {
                    velocity = Vector3.Slerp(forwardVector, input, RotationSpeed * dt) * CurrentSpeed;
                }
            }
            else
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed - Deceleration * dt, 0, CurrentMaxMovementSpeed);
                velocity = velocity.normalized * CurrentSpeed;
            }
        }

        private void RotateBody()
        {
            controlledCharacter.CharacterTransform.rotation = Quaternion.LookRotation(Forward, Vector3.up);
        }

        private void MoveBody(float fdt)
        {
            controlledCharacter.CharacterRigidbody.MovePosition(controlledCharacter.CharacterTransform.position + velocity * fdt);
        }
    }
}