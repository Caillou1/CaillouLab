using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {
	
	void Update ()
    {
        KU.Log(Random.value, 0, Color.red);
        KU.Log(Random.value, 0, Color.green);
        KU.Log(Random.value, 0, Color.blue);
        KU.Log(Random.value, 0, Color.red);
    }
}
