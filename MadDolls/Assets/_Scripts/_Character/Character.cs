using RootMotion.Dynamics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterControllerComponent))]
[RequireComponent(typeof(CharacterHealthComponent))]
[RequireComponent(typeof(CharacterMovementComponent))]
[RequireComponent(typeof(CharacterPickupComponent))]
[RequireComponent(typeof(CharacterInverseKinematicComponent))]
[RequireComponent(typeof(CharacterAnimationComponent))]
public class Character : MonoBehaviour
{
    [Header("Player")]
    public int PlayerID = 0;

    [Header("Character Components")]
    public CharacterControllerComponent CharacterController;
    public CharacterHealthComponent CharacterHealth;
    public CharacterMovementComponent CharacterMovement;
    public CharacterPickupComponent CharacterPickup;
    public CharacterInverseKinematicComponent CharacterIK;
    public CharacterAnimationComponent CharacterAnimation;
    public PuppetMaster PuppetMasterComponent;

    public Transform characterTransform { get; private set; }
    public Rigidbody characterRigidbody { get; private set; }


    private void Awake()
    {
        InitializeCharacter();
        InitializeComponents();
    }

    private void InitializeCharacter()
    {
        characterTransform = transform;
        characterRigidbody = GetComponent<Rigidbody>();
    }

    private void InitializeComponents()
    {
        CharacterController.InitializeComponent(this);
        CharacterHealth.InitializeComponent(this);
        CharacterMovement.InitializeComponent(this);
        CharacterPickup.InitializeComponent(this);
        CharacterIK.InitializeComponent(this);
        CharacterAnimation.InitializeComponent(this);
    }
}
