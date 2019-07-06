using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDirection : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
       transform.rotation = Quaternion.Euler(new Vector3(0,0,1)); 
    }
}
