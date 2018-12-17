using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCharacterUpright : MonoBehaviour
{
    public bool KeepUpRight = true;
    public float UpForce = 10f;
    public float UpOffset = 1.45f;
    public float AdditionalUpForce = 10f;
    public float DampenAngularForce = 0;

    private Rigidbody rigidBody;
    private Transform tf;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        tf = transform;
    }

    void Update()
    {
        if (KeepUpRight)
        {
            rigidBody.AddForceAtPosition(new Vector3(0, (UpForce + AdditionalUpForce), 0), tf.position + tf.TransformPoint(new Vector3(0, UpOffset, 0)), ForceMode.Force);

            rigidBody.AddForceAtPosition(new Vector3(0, -UpForce, 0), tf.position + tf.TransformPoint(new Vector3(0, -UpOffset, 0)), ForceMode.Force);
        }

        if(DampenAngularForce > 0)
        {
            rigidBody.angularVelocity *= (1 - Time.deltaTime * DampenAngularForce);
        }
    }
}
