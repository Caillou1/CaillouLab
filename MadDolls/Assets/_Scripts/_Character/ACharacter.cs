using RootMotion.Dynamics;
using UnityEngine;

namespace Gameplay.Character
{
    public abstract class ACharacter : MonoBehaviour
    {
        [Header("Common Character Parameters")]
        public bool AutoRegisterToGameManager = true;
        public Transform CharacterTransform;
        public Rigidbody CharacterRigidbody;
        public PuppetMaster PuppetMasterComponent;
        public CharacterComponentHealth CharacterHealth;
        public CharacterComponentMovement CharacterMovement;
        public ACharacterComponentController CharacterController;
        public CharacterComponentInverseKinematic CharacterIK;
        public CharacterComponentAnimation CharacterAnimation;
        public CharacterComponentPickup CharacterPickup;

        private void Awake()
        {
            PreInitializeComponents();
        }

        private void Start()
        {
            if(AutoRegisterToGameManager)
                Gameplay.Management.GameManager.Instance.RegisterCharacter(this);
            InitializeComponents();
        }

        private void Update()
        {
            UpdateComponents();
        }

        private void LateUpdate()
        {
            LateUpdateComponents();
        }

        private void FixedUpdate()
        {
            FixedUpdateComponents();
        }

        protected virtual void PreInitializeComponents()
        {
            CharacterHealth.PreInitComponent(this);
            CharacterController.PreInitComponent(this);
            CharacterMovement.PreInitComponent(this);
            CharacterIK.PreInitComponent(this);
            CharacterAnimation.PreInitComponent(this);
            CharacterPickup.PreInitComponent(this);
        }

        protected virtual void InitializeComponents()
        {
            CharacterHealth.InitComponent();
            CharacterController.InitComponent();
            CharacterMovement.InitComponent();
            CharacterIK.InitComponent();
            CharacterAnimation.InitComponent();
            CharacterPickup.InitComponent();
        }

        protected virtual void UpdateComponents()
        {
            float dt = Time.deltaTime;
            CharacterHealth.UpdateComponent(dt);
            CharacterController.UpdateComponent(dt);
            CharacterMovement.UpdateComponent(dt);
            CharacterIK.UpdateComponent(dt);
            CharacterAnimation.UpdateComponent(dt);
            CharacterPickup.UpdateComponent(dt);
        }

        protected virtual void LateUpdateComponents()
        {
            float dt = Time.deltaTime;
            CharacterHealth.LateUpdateComponent(dt);
            CharacterController.LateUpdateComponent(dt);
            CharacterMovement.LateUpdateComponent(dt);
            CharacterIK.LateUpdateComponent(dt);
            CharacterAnimation.LateUpdateComponent(dt);
            CharacterPickup.LateUpdateComponent(dt);
        }

        protected virtual void FixedUpdateComponents()
        {
            float fdt = Time.fixedDeltaTime;
            CharacterHealth.FixedUpdateComponent(fdt);
            CharacterController.FixedUpdateComponent(fdt);
            CharacterMovement.FixedUpdateComponent(fdt);
            CharacterIK.FixedUpdateComponent(fdt);
            CharacterAnimation.FixedUpdateComponent(fdt);
            CharacterPickup.FixedUpdateComponent(fdt);
        }
    }
}