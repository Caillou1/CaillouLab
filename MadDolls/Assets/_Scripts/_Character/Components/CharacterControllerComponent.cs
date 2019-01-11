using UnityEngine;

public class CharacterControllerComponent : CharacterComponent
{
    public Vector3 LeftStickDirection { get; private set; }
    public Vector3 RightStickDirection { get; private set; }

    private void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        UpdateInputDirections();
        //TO DO : setup inputs + axis
    }

    private void UpdateInputDirections()
    {
        LeftStickDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        RightStickDirection = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")).normalized;
    }
}
