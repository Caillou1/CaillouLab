using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIControllerComponent : CharacterControllerComponent
{
    public Transform Player;
    
    private bool canMove = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) canMove = true;
        if (canMove)
        {
            LeftStickDirection = -Vector3.forward;
        }
    }
}
