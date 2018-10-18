using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour {
    [Header("Movement/Rotation")]
    public float MovementSpeed = 5;
    public float YRotationSpeed = 100;
    public float XRotationSpeed = 100;
    public float JumpForce = 1;

    [Header("Head Bobbing")]
    public bool BobbingEnabled = true;
    public float BobbingSpeedFactor = .18f;
    public float BobbingAmount = .2f;

    private Transform fpsTransform;
    private Rigidbody fpsRigidbody;
    private Transform cameraTransform;
    private Vector3 targetVelocity = Vector3.zero;

    private void Start()
    {
        fpsTransform = transform;
        fpsRigidbody = GetComponent<Rigidbody>();
        cameraTransform = fpsTransform.GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        KU.Raycast(fpsTransform.position, cameraTransform.forward, 100f);
        ApplyRotation();
        if (BobbingEnabled)
        {
            ApplyHeadBobbing();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyHeadBobbing()
    {
    }

    private void ApplyRotation()
    {
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * XRotationSpeed;
        float y = -Input.GetAxis("Mouse Y") * Time.deltaTime * YRotationSpeed;

        fpsTransform.Rotate(0, x, 0);
        cameraTransform.Rotate(y, 0, 0);
    }

    private void ApplyMovement()
    {
        targetVelocity = fpsTransform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * MovementSpeed * Time.fixedDeltaTime;
        fpsRigidbody.MovePosition(fpsRigidbody.position + targetVelocity);
        if (Input.GetButtonDown("Jump"))
            fpsRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
}
