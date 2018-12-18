using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public float MovementSpeed = 10f;

    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform LeftFootTargetTransform;
    public Transform RightFootTargetTransform;

    private Rigidbody rigidBody;
    private Transform tf;
    private Vector3 inputDirection;

    bool CurrentFootIsRight = true;

    void Start()
    {
        tf = transform;
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(ComputeFootTarget());
    }
    
    void Update()
    {
        Move();
    }

    IEnumerator ComputeFootTarget()
    {
        while (true)
        {
            var footTarget = CurrentFootIsRight ? RightFootTargetTransform : LeftFootTargetTransform;
            var otherTarget = CurrentFootIsRight ? LeftFootTargetTransform : RightFootTargetTransform;
            var other = CurrentFootIsRight ? LeftFoot : RightFoot;
            var foot = CurrentFootIsRight ? RightFoot : LeftFoot;
            var startPos = tf.position;
            float factor = 1;
            yield return new WaitUntil(() => {
                footTarget.position = new Vector3(tf.position.x, footTarget.position.y, tf.position.z) + inputDirection.normalized * factor;
                otherTarget.position = other.position;
                return (footTarget.position - foot.position).magnitude <= .5f;
                });
            CurrentFootIsRight = !CurrentFootIsRight;
        }
    }

    private void Move()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rigidBody.MovePosition(tf.position + inputDirection * Time.deltaTime * MovementSpeed);
    }
}
