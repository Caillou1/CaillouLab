using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CharacterControllerComponent : CharacterComponent
{
    [Header("Debug")]
    public bool DebugDirections = true;
    public Vector3 LeftStickDirection { get; protected set; }
    public Vector3 RightStickDirection { get; protected set; }

    protected void DebugInputDirections() {
        var pos = controlledCharacter.characterTransform.position;
        Debug.DrawLine(pos, pos + LeftStickDirection, Color.blue, Time.deltaTime);
        Debug.DrawLine(pos, pos + RightStickDirection, Color.red, Time.deltaTime);
    }
}
