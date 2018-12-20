using UnityEngine;

public class CharacterMovementComponent : CharacterComponent
{
    public float MaxMovementSpeed = 5f;
    public float Acceleration = 5f;
    public float Deceleration = 5f;
    public float RotationSpeed = 5f;

    private Vector3 velocity;
    public float CurrentSpeed { get; private set; }
    
    public Vector3 Forward { get
        {
            return (velocity.magnitude > .01f) ? velocity.normalized : controlledCharacter.characterTransform.forward;
        }
    }

    private void Update()
    {
        UpdateSpeed();
        RotateBody();
    }

    private void UpdateSpeed()
    {
        var input = controlledCharacter.CharacterController.LeftStickDirection;
        if (input.magnitude > 0.05f)
        {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed + Acceleration * Time.deltaTime, 0, MaxMovementSpeed);
            velocity = Vector3.Slerp(velocity.normalized, input, RotationSpeed * Time.deltaTime) * CurrentSpeed;
        }
        else
        {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed - Deceleration * Time.deltaTime, 0, MaxMovementSpeed);
            velocity = velocity.normalized * CurrentSpeed;
        }
    }

    private void FixedUpdate()
    {
        MoveBody();
    }

    private void RotateBody()
    {
        controlledCharacter.characterTransform.rotation = Quaternion.LookRotation(Forward, Vector3.up);
    }

    private void MoveBody()
    {
        controlledCharacter.characterRigidbody.MovePosition(controlledCharacter.characterTransform.position + velocity * Time.fixedDeltaTime);
    }
}
