﻿using UnityEngine;

public class CharacterMovementComponent : CharacterComponent
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
    public float CurrentMaxMovementSpeed {
        get;
        private set;
    }
    public float CurrentSpeed { get; private set; }
    
    public Vector3 Forward {
        get
        {
            if(controlledCharacter.CharacterPickup.HasObjectInHands) {
                return controlledCharacter.CharacterIK.AimDirection;
            } else {
                return (velocity.magnitude > .01f) ? velocity.normalized : controlledCharacter.characterTransform.forward;
            }
        }
    }

    public Vector3 Right {
        get {
            return controlledCharacter.characterTransform.right;
        }
    }

    public float TurnRate {
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

    private float currentTurnRate { get; set; }

    public bool IsGrounded
    {
        get
        {
            return Physics.Raycast(controlledCharacter.characterTransform.position + Vector3.up, Vector3.down, 1.25f, GroundLayer);
        }
    }

    private void Update()
    {
        UpdateSpeed();
        RotateBody();
    }

    private void UpdateSpeed()
    {
        CurrentMaxMovementSpeed = controlledCharacter.CharacterPickup.HasObjectInHands ? MaxAimingMovementSpeed : MaxMovementSpeed;
        var input = controlledCharacter.CharacterController.LeftStickDirection;
        if (input.magnitude > 0.05f)
        {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed + Acceleration * Time.deltaTime, 0, CurrentMaxMovementSpeed);
            var forwardVector = velocity.magnitude > 0.05f ? velocity.normalized : controlledCharacter.characterTransform.forward;
            if(controlledCharacter.CharacterPickup.HasObjectInHands) {
                velocity = Vector3.Lerp(forwardVector, input, RotationSpeed * 5 * Time.deltaTime) * CurrentSpeed;
            } else {
                velocity = Vector3.Slerp(forwardVector, input, RotationSpeed * Time.deltaTime) * CurrentSpeed;
            }
        }
        else
        {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed - Deceleration * Time.deltaTime, 0, CurrentMaxMovementSpeed);
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
