﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Character
{
    [System.Serializable]
    public abstract class ACharacterComponentController : ACharacterComponent
    {
        [Header("Debug")]
        public bool DebugDirections = true;

        public Vector3 LeftStickDirection { get; protected set; }
        public Vector3 RightStickDirection { get; protected set; }

        protected void DebugInputDirections()
        {
            var pos = controlledCharacter.CharacterTransform.position;
            Debug.DrawLine(pos, pos + LeftStickDirection, Color.blue, Time.deltaTime);
            Debug.DrawLine(pos, pos + RightStickDirection, Color.red, Time.deltaTime);
        }
    }
}