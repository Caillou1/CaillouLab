using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotator : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Transform billboardCanvasTransform;

    private void Awake()
    {
        mainCameraTransform = Camera.main.transform;
        billboardCanvasTransform = transform;
    }

    private void LateUpdate()
    {
        var dir = (billboardCanvasTransform.position - mainCameraTransform.position).normalized;
        billboardCanvasTransform.rotation = Quaternion.LookRotation(dir);
    }
}
