using RootMotion.FinalIK;
using System.Linq;
using UnityEngine;

public class CharacterInverseKinematicComponent : CharacterComponent
{
    [Header("Debug")]
    public bool EnableFootCycleDebug = true;
    public bool EnableAimDebug = true;

    [Header("Aim System")]
    public AimIK ShootAimIK;
    public float AimAssist = .03f;
    public float AimSpeed = 3f;
    public float AimAssistSpeed = 5f;

    public Vector3 AimDirection {
        get
        {
            Vector3 dir = aimPos - controlledCharacter.CharacterTransform.position;
            dir.y = 0;
            return dir.normalized;
        }
    }

    public Vector3 RealAimDirection
    {
        get
        {
            Vector3 dir = realAimPos - controlledCharacter.CharacterTransform.position;
            dir.y = 0;
            return dir.normalized;
        }
    }

    private Vector3 aimPos;
    private Vector3 realAimPos;
    private Vector3 newAimPos
    {
        get
        {
            if (controlledCharacter.CharacterController.RightStickDirection.magnitude > .01f)
            {
                return ShootAimIK.solver.transform.position + controlledCharacter.CharacterController.RightStickDirection * 5f;
            }
            else
            {
                return ShootAimIK.solver.transform.position + controlledCharacter.CharacterMovement.Forward * 5f;
            }
        }
    }
    
    public bool IsAimingEnabled
    {
        get;
        private set;
    }

    private void Update()
    {
        if(IsAimingEnabled)
        {
            ComputeAiming();
        }
    }

    private void ComputeAiming()
    {
        aimPos = Vector3.Slerp(aimPos, newAimPos, Time.deltaTime * AimSpeed);

        var aimedCharacter = FindAimedCharacter();
        if (aimedCharacter != null) {
            realAimPos = Vector3.Lerp(realAimPos, aimedCharacter.CharacterTransform.position, Time.deltaTime * AimAssistSpeed);
        } else
        {
            realAimPos = Vector3.Lerp(realAimPos, aimPos, Time.deltaTime * AimSpeed);
        }

        ShootAimIK.solver.IKPosition = realAimPos;

        if (EnableAimDebug)
        {
            KU.DebugSphere(newAimPos, .5f, Color.red, Time.deltaTime);
            KU.DebugSphere(aimPos, .33f, Color.yellow, Time.deltaTime);
            KU.DebugSphere(realAimPos, .25f, Color.green, Time.deltaTime);
        }
    }

    private Character FindAimedCharacter()
    {
        var opponents = GameManager.Instance.GetOpponents(controlledCharacter);
        Character target = null;

        if (opponents.Count > 0)
        {
            var bestTarget = opponents.OrderByDescending(c =>
            {
                var dir = c.CharacterTransform.position - controlledCharacter.CharacterTransform.position;
                dir.y = 0;
                dir.Normalize();

                return Vector3.Dot(dir, AimDirection);
            }).ElementAt(0);

            var bestDir = bestTarget.CharacterTransform.position - controlledCharacter.CharacterTransform.position;
            bestDir.y = 0;
            bestDir.Normalize();

            if (Vector3.Dot(bestDir, AimDirection) >= (1 - AimAssist))
            {
                target = bestTarget;
            }
        }

        return target;
    }

    public void EnableAiming()
    {
        aimPos = ShootAimIK.solver.transform.position + controlledCharacter.CharacterMovement.Forward * 5f;
        ShootAimIK.enabled = true;
        IsAimingEnabled = true;
    }

    public void DisableAiming()
    {
        ShootAimIK.enabled = false;
        IsAimingEnabled = false;
    }
}
