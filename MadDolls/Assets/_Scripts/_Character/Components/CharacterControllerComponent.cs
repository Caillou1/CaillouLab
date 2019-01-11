using UnityEngine;

public class CharacterControllerComponent : CharacterComponent
{
    public bool DebugDirections = true;
    public Vector3 LeftStickDirection { get; private set; }
    public Vector3 RightStickDirection { get; private set; }

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
        RightStickDirection = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")).normalized;
    }
}
