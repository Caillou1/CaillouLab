using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIControllerComponent : CharacterControllerComponent
{
    public LayerMask DefaultLayer;
    private Vector3 newDirection;

    private void Start()
    {
        newDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        KU.StartTimer(() => newDirection = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, UnityEngine.Random.Range(-1f, 1f)), 5, true);
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(controlledCharacter.CharacterTransform.position, controlledCharacter.CharacterMovement.Forward, out hit, 2f, DefaultLayer))
        {
            newDirection = hit.normal;
            newDirection.y = 0;
            newDirection.Normalize();
        }

        LeftStickDirection = newDirection.normalized;
    }
}
