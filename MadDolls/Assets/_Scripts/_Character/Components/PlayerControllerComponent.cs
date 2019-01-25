using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class PlayerControllerComponent : CharacterControllerComponent
{
    public bool UseKeyboardMouse = true;

    private Action getRightStickAction;
    private Camera mainCamera;

    private void Awake()
    {
        Action keyboardAction = SetRightStickFromKeyboard;
        Action gamepadAction = SetRightStickFromGamepad;
        getRightStickAction = UseKeyboardMouse ? keyboardAction : gamepadAction;

        mainCamera = Camera.main;
    }

    private void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        UpdateInputDirections();

        if (Input.GetButtonDown("PickUp"))
        {
            controlledCharacter.CharacterPickup.PickUp();
        }

        if (Input.GetButtonDown("Fire"))
        {
            if (controlledCharacter.CharacterPickup.HasObjectInHands)
            {
                controlledCharacter.CharacterPickup.pickedupObject.StartUse();
            }
        }

        if (Input.GetButtonUp("Fire"))
        {
            if (controlledCharacter.CharacterPickup.HasObjectInHands)
            {
                controlledCharacter.CharacterPickup.pickedupObject.EndUse();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            controlledCharacter.CharacterIK.EnableAiming();
        }
        if (Input.GetMouseButtonUp(1))
        {
            controlledCharacter.CharacterIK.DisableAiming();
        }

        if (DebugDirections)
            DebugInputDirections();
    }

    private void SetRightStickFromGamepad()
    {
        RightStickDirection = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")).normalized;
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
        LeftStickDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        getRightStickAction.Invoke();
    }
}
