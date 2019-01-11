using System;
using UnityEngine;

public class CharacterControllerComponent : CharacterComponent
{
    public bool UseKeyboardMouse = true;
    public bool DebugDirections = true;
    public Vector3 LeftStickDirection { get; private set; }
    public Vector3 RightStickDirection { get; private set; }

    private Action getRightStickAction;
    private Camera mainCamera;

    private void Awake() {
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

        if(DebugDirections)
            DebugInputDirections();
        //TO DO : setup inputs + axis
    }

    private void DebugInputDirections() {
        var pos = controlledCharacter.characterTransform.position;
        Debug.DrawLine(pos, pos + LeftStickDirection, Color.blue, Time.deltaTime);
        Debug.DrawLine(pos, pos + RightStickDirection, Color.red, Time.deltaTime);
    }

    private void UpdateInputDirections()
    {
        LeftStickDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        getRightStickAction.Invoke();
    }

    private void SetRightStickFromGamepad() {
        RightStickDirection = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")).normalized;
    }

    private void SetRightStickFromKeyboard() {
        Vector2 controlledCharacterPositionOnScreen = mainCamera.WorldToScreenPoint(controlledCharacter.characterTransform.position);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 dir = (mousePosition - controlledCharacterPositionOnScreen).normalized;
        RightStickDirection = new Vector3(dir.x, 0, dir.y);
    }
}
