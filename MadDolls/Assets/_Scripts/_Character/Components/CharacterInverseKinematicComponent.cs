using RootMotion.FinalIK;
using UnityEngine;

public class CharacterInverseKinematicComponent : CharacterComponent
{
    [Header("Debug")]
    public bool EnableFootCycleDebug = true;
    public bool EnableAimDebug = true;

    [Header("Aim System")]
    public AimIK ShootAimIK;

    [Header("Foot Cycle System")]
    public Transform FootCycleOrigin;
    public float MaxFootCycleRadius = .5f;
    public float MaxFootCycleRotationSpeed = 50f;

    public Vector3 AimDirection {
        get
        {
            Vector3 dir = aimPos - controlledCharacter.characterTransform.position;
            dir.y = 0;
            return dir.normalized;
        }
    }

    private Vector3 aimPos;
    private Vector2 footCycleOrigin;
    private float footCycleRadius;
    private float footCycleRotation;
    
    private bool IsAimingEnabled = false;

    private void Update()
    {
        if(IsAimingEnabled)
        {
            Vector3 newPos;
            //ShootAimIK.solver.polePosition = controlledCharacter.characterTransform.position + new Vector3(0, 10, 0);

            if (controlledCharacter.CharacterController.RightStickDirection.magnitude > .01f)
            {
                newPos = ShootAimIK.solver.transform.position + controlledCharacter.CharacterController.RightStickDirection * 5f;
            } else
            {
                newPos = ShootAimIK.solver.transform.position + controlledCharacter.CharacterMovement.Forward * 5f;
            }
            aimPos = Vector3.Slerp(ShootAimIK.solver.IKPosition, newPos, Time.deltaTime * 3f);
            ShootAimIK.solver.IKPosition = aimPos;
            
            if(EnableAimDebug)
            {
                KU.DebugSphere(newPos, .5f, Color.green, Time.deltaTime);
                KU.DebugSphere(ShootAimIK.solver.IKPosition, .25f, Color.red, Time.deltaTime);
            }
        }
    }

    public void EnableAiming()
    {
        ShootAimIK.solver.IKPosition = ShootAimIK.solver.transform.position + controlledCharacter.CharacterMovement.Forward * 5f;;
        ShootAimIK.enabled = true;
        IsAimingEnabled = true;
    }

    public void DisableAiming()
    {
        ShootAimIK.enabled = false;
        IsAimingEnabled = false;
    }
}
