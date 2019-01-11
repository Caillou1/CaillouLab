using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform[] TransformsToFollow;
    public Vector3 CameraOffset;

    private Transform tf;

    void Start()
    {
        tf = transform;
    }
    
    void Update()
    {
        Vector3 averageFollowPosition = Vector3.zero;
        foreach(var t in TransformsToFollow)
        {
            averageFollowPosition += t.position;
        }
        averageFollowPosition /= TransformsToFollow.Length;

        tf.position = Vector3.Lerp(tf.position, averageFollowPosition + CameraOffset, Time.deltaTime);
    }
}
