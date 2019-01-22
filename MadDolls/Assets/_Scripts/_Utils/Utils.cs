using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator Timer(Action action, float duration, bool loop)
    {
        do
        {
            yield return new WaitForSeconds(duration);
            action.Invoke();
        } while (loop);
    }

    static float sphereT = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
    static List<Vector3> sphereVertices = new List<Vector3>
    {
        new Vector3(-1,  sphereT,  0),
        new Vector3(1, sphereT, 0),
        new Vector3(-1, -sphereT, 0),
        new Vector3(1, -sphereT, 0),
        new Vector3(0, -1, sphereT),
        new Vector3(0, 1, sphereT),
        new Vector3(0, -1, -sphereT),
        new Vector3(0, 1, -sphereT),
        new Vector3(sphereT, 0, -1),
        new Vector3(sphereT, 0, 1),
        new Vector3(-sphereT, 0, -1),
        new Vector3(-sphereT, 0, 1)
    };
    static List<int> sphereTriangles = new List<int>
    {
        0,11,5,0,5,1,0,1,7,0,7,10,0,10,11,1,5,9,5,11,4,11,10,2,10,7,6,7,1,8,3,9,4,3,4,2,3,2,6,3,6,8,3,8,9,4,9,5,2,4,11,6,2,10,8,6,7,9,8,1
    };

    public static void DebugSphere(Vector3 center, float radius, Color color, float duration)
    {
        radius /= 2f;
        for(int i = 0; i< sphereTriangles.Count - 3; i+=3)
        {
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i]] * radius, center + sphereVertices[sphereTriangles[i + 1]] * radius, color, duration);
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i+1]] * radius, center + sphereVertices[sphereTriangles[i + 2]] * radius, color, duration);
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i+2]] * radius, center + sphereVertices[sphereTriangles[i]] * radius, color, duration);
        }
    }
}