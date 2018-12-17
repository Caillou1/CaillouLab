using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public float MovementSpeed = 10f;

    private Rigidbody rigidBody;
    private Transform tf;
    private Vector3 inputDirection;

    void Start()
    {
        tf = transform;
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rigidBody.MovePosition(tf.position + inputDirection * Time.deltaTime * MovementSpeed);
    }
}
