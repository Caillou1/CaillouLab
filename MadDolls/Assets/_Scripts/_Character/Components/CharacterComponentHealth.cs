using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Character
{
    [System.Serializable]
    public class CharacterComponentHealth : ACharacterComponent
    {
        public int MaxHealth;
        public bool UseTraumaInsteadOfHealth = true;
        public GameObject HealthBarObject;
        public Slider HealthBarGreenSlider;
        public Slider HealthBarRedSlider;
        public int CurrentHealth { get; private set; }
        public float TimeBeforeDecrementRedSlider = 1f;
        public Collider DamageCollider;

        public bool IsAlive
        {
            get
            {
                return CurrentHealth > 0;
            }
        }

        private float LastHealthLostTime;

        public override void InitComponent()
        {
            CurrentHealth = MaxHealth;
            HealthBarGreenSlider.value = 1;
        }

        public override void UpdateComponent(float deltaTime)
        {
            if (HealthBarObject)
            {
                if (HealthBarRedSlider.value != HealthBarGreenSlider.value && (Time.time - LastHealthLostTime) >= TimeBeforeDecrementRedSlider)
                {
                    HealthBarRedSlider.value = Mathf.LerpUnclamped(HealthBarRedSlider.value, HealthBarGreenSlider.value, Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) ApplyDamage(0);
            UpdateTrauma(deltaTime);
        }

        [Range(0f, 1f)] [SerializeField] private float trauma;
        [SerializeField] private float traumaMultiplier = 5f;
        [SerializeField] private float traumaPow = .3f;
        [SerializeField] private float traumaDecay = 1.3f;

        private float timeCounter;

        public void UpdateTrauma(float deltaTime)
        {
            if (trauma > 0)
            {
                timeCounter += deltaTime * Mathf.Pow(trauma, traumaPow) * traumaMultiplier;
                trauma -= deltaTime * traumaDecay * trauma;
                if (trauma < .01f) trauma = 0;
            }
            controlledCharacter.PuppetMasterComponent.muscleWeight = 1 - trauma;
            controlledCharacter.PuppetMasterComponent.pinWeight = 1 - trauma;
        }

        public void ApplyDamage(int amount)
        {
            if (UseTraumaInsteadOfHealth)
            {
                trauma = Mathf.Clamp01(trauma + .2f);
            }
            else if (CurrentHealth > 0)
            {
                CurrentHealth -= amount;
                HealthBarGreenSlider.value = (float)CurrentHealth / MaxHealth;
                LastHealthLostTime = Time.time;
                if (CurrentHealth <= 0)
                {
                    Kill();
                }
            }
        }

        public void ApplyDamage(int amount, Vector3 dir)
        {
            ApplyDamage(amount);
            controlledCharacter.PuppetMasterComponent.pinWeight = 0;
            KU.StartTimer(() => controlledCharacter.PuppetMasterComponent.pinWeight = 1, 1);
            foreach (var rb in controlledCharacter.PuppetMasterComponent.transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
            {
                rb.AddForce(dir * amount, ForceMode.Impulse);
            }
        }

        public void Kill()
        {
            controlledCharacter.PuppetMasterComponent.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            CameraFollow.Instance.UnregisterTarget(controlledCharacter.CharacterTransform);
            HealthBarObject.SetActive(false);
            DamageCollider.enabled = false;
        }
    }
}