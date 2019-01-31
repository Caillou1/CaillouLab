using RootMotion.Dynamics;
using UnityEngine;

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

    public Transform CharacterTransform;
    public Rigidbody CharacterRigidbody;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        GameManager.Instance.RegisterCharacter(this);
    }

    private void InitializeCharacter(int playerID)
    {
        PlayerID = playerID;
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
