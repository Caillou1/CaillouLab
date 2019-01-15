using RootMotion.FinalIK;
using UnityEngine;

public class CharacterInverseKinematicComponent : CharacterComponent
{
    public bool EnableFootCycleDebug = true;
    public Transform FootCycleOrigin;
    public float MaxFootCycleRadius = .5f;
    public float MaxFootCycleRotationSpeed = 50f;

    private Vector2 footCycleOrigin;
    private float footCycleRadius;
    private float footCycleRotation;

    public AimIK ShootAimIK;
    public Transform ShootAimTarget;

    private bool IsAimingEnabled = false;

    private void Update()
    {
        UpdateFootCycle();

        if (EnableFootCycleDebug)
        {
            DebugFootCycle();
        }

        if(IsAimingEnabled)
        {
            ShootAimIK.solver.polePosition = controlledCharacter.characterTransform.position + new Vector3(0, 10, 0);
            if (controlledCharacter.CharacterController.RightStickDirection.magnitude > .01f)
            {
                Vector3 newPos = controlledCharacter.characterTransform.position + controlledCharacter.CharacterController.RightStickDirection * 5f;
                ShootAimIK.solver.IKPosition = Vector3.LerpUnclamped(ShootAimIK.solver.IKPosition, newPos, Time.deltaTime * 3f);
            }
        }
    }

    public void EnableAiming()
    {
        ShootAimIK.solver.transform = controlledCharacter.CharacterPickup.pickedupObject.GetTransform();
        ShootAimIK.enabled = true;
        IsAimingEnabled = true;
    }

    private void DebugFootCycle()
    {
        //circle
        float step = -360f / 16;
        for (int i = 0; i < 16; i++)
        {
            int next = (i + 1) % 16;
            float angle = footCycleRotation - i * step;
            float nextAngle = footCycleRotation - next * step;
            float x, y, nextX, nextY;
            x = -Mathf.Sin(angle * Mathf.Deg2Rad) * footCycleRadius;
            nextX = -Mathf.Sin(nextAngle * Mathf.Deg2Rad) * footCycleRadius;
            y = Mathf.Cos(angle * Mathf.Deg2Rad) * footCycleRadius;
            nextY = Mathf.Cos(nextAngle * Mathf.Deg2Rad) * footCycleRadius;
            Debug.DrawLine(FootCycleOrigin.position + FootCycleOrigin.TransformDirection(new Vector3(0, y, x)), FootCycleOrigin.position + FootCycleOrigin.TransformDirection(new Vector3(0, nextY, nextX)), Color.green, Time.deltaTime);
        }

        //foots
        Vector3 leftFootPos1, leftFootPos2, rightFootPos1, rightFootPos2;
        leftFootPos1 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos(-footCycleRotation * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin(-footCycleRotation * Mathf.Deg2Rad) * footCycleRadius));
        leftFootPos2 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 180) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 180) * Mathf.Deg2Rad) * footCycleRadius));
        rightFootPos1 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 90) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 90) * Mathf.Deg2Rad) * footCycleRadius));
        rightFootPos2 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 270) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 270) * Mathf.Deg2Rad) * footCycleRadius));

        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + leftFootPos1, Color.blue, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + leftFootPos2, Color.blue, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + rightFootPos1, Color.red, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + rightFootPos2, Color.red, Time.deltaTime);
    }

    private void UpdateFootCycle()
    {
        float currentSpeed = controlledCharacter.CharacterMovement.CurrentSpeed;
        float maxSpeed = controlledCharacter.CharacterMovement.MaxMovementSpeed;

        if (currentSpeed == 0)
        {
            footCycleOrigin = Vector2.up;
            footCycleRadius = 0f;
        }
        else
        {
            float speedFactor = currentSpeed / maxSpeed;
            footCycleRadius = speedFactor * MaxFootCycleRadius;
            footCycleRotation = (footCycleRotation + speedFactor * MaxFootCycleRotationSpeed * Time.deltaTime) % 360;
        }
    }
}
