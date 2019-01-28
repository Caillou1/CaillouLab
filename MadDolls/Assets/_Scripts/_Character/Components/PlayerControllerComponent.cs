using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class PlayerControllerComponent : CharacterControllerComponent
{
    public bool UseKeyboardMouse = true;

    private Action getRightStickAction;
    private Camera mainCamera;
    private Player player;

    private void Awake()
    {
        Action keyboardAction = SetRightStickFromKeyboard;
        Action gamepadAction = SetRightStickFromGamepad;
        getRightStickAction = UseKeyboardMouse ? keyboardAction : gamepadAction;

        mainCamera = Camera.main;
    }

    private void Start() {
        player = ReInput.players.GetPlayer(controlledCharacter.PlayerID);
    }

    private void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        UpdateInputDirections();

        if (player.GetButtonDown("Y") || Input.GetButtonDown("PickUp"))
        {
            controlledCharacter.CharacterPickup.PickUp();
        }

        if (player.GetButtonDown("RightTrigger") || Input.GetButtonDown("Fire"))
        {
            if (controlledCharacter.CharacterPickup.HasObjectInHands)
            {
                controlledCharacter.CharacterPickup.pickedupObject.StartUse();
            }
        }

        if (player.GetButtonUp("RightTrigger") || Input.GetButtonUp("Fire"))
        {
            if (controlledCharacter.CharacterPickup.HasObjectInHands)
            {
                controlledCharacter.CharacterPickup.pickedupObject.EndUse();
            }
        }

        if (player.GetButtonDown("LeftTrigger") || Input.GetMouseButtonDown(1))
        {
            controlledCharacter.CharacterIK.EnableAiming();
        }
        if (player.GetButtonUp("LeftTrigger") || Input.GetMouseButtonUp(1))
        {
            controlledCharacter.CharacterIK.DisableAiming();
        }

        if (DebugDirections)
            DebugInputDirections();
    }

    private void SetRightStickFromGamepad()
    {
        RightStickDirection = new Vector3(player.GetAxis("RightHorizontal"), 0, player.GetAxis("RightVertical")).normalized;
    }

    private void SetRightStickFromKeyboard()
    {
        Vector2 controlledCharacterPositionOnScreen = mainCamera.WorldToScreenPoint(controlledCharacter.characterTransform.position);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 dir = (mousePosition - controlledCharacterPositionOnScreen).normalized;
        RightStickDirection = new Vector3(dir.x, 0, dir.y);
    }

    private void UpdateInputDirections()
    {
        LeftStickDirection = new Vector3(player.GetAxis("LeftHorizontal"), 0, player.GetAxis("LeftVertical")).normalized;
        getRightStickAction.Invoke();
    }
}
