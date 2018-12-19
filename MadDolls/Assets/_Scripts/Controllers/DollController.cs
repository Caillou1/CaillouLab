using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public float MaxMovementSpeed = 5f;
    public float Acceleration = 5f;
    public float Deceleration = 5f;
    public float RotationSpeed = 5f;

    private Vector3 velocity;
    private float currentSpeed;
    private Rigidbody rigidBody;
    private Transform tf;

    //Foot cycle system
    public bool EnableFootCycleDebug = true;
    public Transform FootCycleOrigin;
    public float MaxFootCycleRadius = .5f;
    public float MaxFootCycleRotationSpeed = 50f;
    private Vector2 footCycleOrigin;
    private float footCycleRadius;
    private float footCycleRotation;

    //Pickup System
    public float PickupRadius = 2f;

    public Vector3 InputDirection { get; private set; }
    public Vector3 Forward { get
        {
            return (velocity.magnitude > .01f) ? velocity.normalized : tf.forward;
        }
    }

    private void Awake()
    {
        tf = transform;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateInputDirection();
        UpdateSpeed();
        RotateBody();
        UpdateFootCycle();
        CheckForObjects();

        if(EnableFootCycleDebug)
            DebugFootCycle();
    }

    private void CheckForObjects()
    {
        //WIP
        var pickups = Physics.OverlapSphere(tf.position, PickupRadius, LayerMask.GetMask("Pickup"));
        var closests = pickups.Min(c => (c.ClosestPoint(tf.position) - tf.position).magnitude);
    }

    private void DebugFootCycle()
    {
        //circle
        float step = -360f / 16;
        for (int i = 0; i < 16; i++)
        {
            int next = (i + 1) % 16;
            float angle = footCycleRotation - i * step;
            float nextAngle = footCycleRotation - next * step;
            float x, y, nextX, nextY;
            x = -Mathf.Sin(angle * Mathf.Deg2Rad) * footCycleRadius;
            nextX = -Mathf.Sin(nextAngle * Mathf.Deg2Rad) * footCycleRadius;
            y = Mathf.Cos(angle * Mathf.Deg2Rad) * footCycleRadius;
            nextY = Mathf.Cos(nextAngle * Mathf.Deg2Rad) * footCycleRadius;
            Debug.DrawLine(FootCycleOrigin.position + FootCycleOrigin.TransformDirection(new Vector3(0, y, x)), FootCycleOrigin.position + FootCycleOrigin.TransformDirection(new Vector3(0, nextY, nextX)), Color.green, Time.deltaTime);
        }

        //foots
        Vector3 leftFootPos1, leftFootPos2, rightFootPos1, rightFootPos2;
        leftFootPos1 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos(-footCycleRotation * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin(-footCycleRotation * Mathf.Deg2Rad) * footCycleRadius));
        leftFootPos2 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 180) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 180) * Mathf.Deg2Rad) * footCycleRadius));
        rightFootPos1 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 90) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 90) * Mathf.Deg2Rad) * footCycleRadius));
        rightFootPos2 = FootCycleOrigin.TransformDirection(new Vector3(0, Mathf.Cos((-footCycleRotation - 270) * Mathf.Deg2Rad) * footCycleRadius, -Mathf.Sin((-footCycleRotation - 270) * Mathf.Deg2Rad) * footCycleRadius));

        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + leftFootPos1, Color.blue, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + leftFootPos2, Color.blue, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + rightFootPos1, Color.red, Time.deltaTime);
        Debug.DrawLine(FootCycleOrigin.position, FootCycleOrigin.position + rightFootPos2, Color.red, Time.deltaTime);
    }

    private void UpdateFootCycle()
    {
        if(currentSpeed == 0 && false)
        {
            footCycleOrigin = Vector2.up;
            footCycleRadius = 0f;
        }
        else
        {
            float speedFactor = currentSpeed / MaxMovementSpeed;
            footCycleRadius = speedFactor * MaxFootCycleRadius;
            footCycleRotation = (footCycleRotation + speedFactor * MaxFootCycleRotationSpeed * Time.deltaTime) % 360;
        }
    }

    private void UpdateSpeed()
    {
        if (InputDirection.magnitude > 0.05f)
        {
            currentSpeed = Mathf.Clamp(currentSpeed + Acceleration * Time.deltaTime, 0, MaxMovementSpeed);
            velocity = Vector3.Slerp(velocity.normalized, InputDirection, RotationSpeed * Time.deltaTime) * currentSpeed;
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed - Deceleration * Time.deltaTime, 0, MaxMovementSpeed);
            velocity = velocity.normalized * currentSpeed;
        }
    }

    private void FixedUpdate()
    {
        MoveBody();
    }

    private void RotateBody()
    {
        //rigidBody.MoveRotation(Quaternion.LookRotation((velocity.magnitude > .01f) ? velocity.normalized : tf.forward, Vector3.up));
        tf.rotation = Quaternion.LookRotation(Forward, Vector3.up);
    }

    private void MoveBody()
    {
        rigidBody.MovePosition(tf.position + velocity * Time.fixedDeltaTime);
    }

    private void UpdateInputDirection()
    {
        InputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}
