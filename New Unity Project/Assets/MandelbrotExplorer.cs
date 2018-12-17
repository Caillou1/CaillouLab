using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandelbrotExplorer : MonoBehaviour
{
    public Material MandelbrotMaterial;
    public int maxIter = 64;
    public float scale = 5f, offsetX = -0.5f, offsetY = 0.0f, speed = 0.5f;
    Vector4 transformation = new Vector4();

    void Update()
    {
        if (Input.GetKey(KeyCode.S)) offsetY += scale * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Z)) offsetY -= scale * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) offsetX -= scale * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q)) offsetX += scale * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift)) scale -= 0.98f * scale * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) scale += 1.02f * scale * Time.deltaTime;
        if (scale > 20.0f) scale = 20.0f;
        if (scale < 0.00002f) scale = 0.00002f;
        if (Input.mouseScrollDelta.y != 0)
        {
            int iter = maxIter + (int)Input.mouseScrollDelta.y;
            if (iter > 16384) maxIter = 16384;
            else if (iter < 1) maxIter = 1;
            else maxIter = iter;
        }
        if (Input.GetKey(KeyCode.R))
        {
            scale = 2.5f;
            offsetX = -0.5f;
            offsetY = 0.0f;
            speed = 0.5f;
            maxIter = 512;
        }
        transformation.x = transformation.y = scale;
        transformation.z = offsetX;
        transformation.w = offsetY;
        MandelbrotMaterial.SetVector("_ST", transformation);
        MandelbrotMaterial.SetFloat("_maxIter", maxIter);
        MandelbrotMaterial.SetFloat("_aspectRatio", 1);
    }
}