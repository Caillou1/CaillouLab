using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIControllerComponent : CharacterControllerComponent
{
    private Vector3 newDirection;

    private void Start()
    {
        KU.StartTimer(() => newDirection = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, UnityEngine.Random.Range(-1f, 1f)), 5, true);
    }

    private void Update()
    {
        LeftStickDirection = newDirection.normalized;
    }
}
