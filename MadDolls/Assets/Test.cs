using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool DisableGravity = true;

    void Start()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        Process(rbs);
    }

    void Process(Rigidbody[] rbs)
    {
        foreach(var rb in rbs)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            var newRbs = rb.GetComponentsInChildren<Rigidbody>();
            Process(newRbs);
        }
    }
}
