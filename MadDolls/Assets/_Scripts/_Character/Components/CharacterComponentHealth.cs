﻿using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Character
{
    [System.Serializable]
    public class CharacterComponentHealth : ACharacterComponent
    {
        public int MaxHealth;
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
            if (HealthBarRedSlider.value != HealthBarGreenSlider.value && (Time.time - LastHealthLostTime) >= TimeBeforeDecrementRedSlider)
            {
                HealthBarRedSlider.value = Mathf.LerpUnclamped(HealthBarRedSlider.value, HealthBarGreenSlider.value, Time.deltaTime);
            }
        }

        public void ApplyDamage(int amount)
        {
            if (CurrentHealth > 0)
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