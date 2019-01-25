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
            var vec = (Player.position - controlledCharacter.characterTransform.position);
            if (vec.magnitude > 5)
            {
                vec.y = 0;
                LeftStickDirection = vec.normalized;
            }
            else
            {
                LeftStickDirection = Vector3.zero;
            }
        }
    }
}
