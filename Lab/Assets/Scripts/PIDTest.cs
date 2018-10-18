using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PIDTest : MonoBehaviour {
    public Transform Target;
    public float p, i, d;
    public float MaxRotSpeed;
    public float minI, maxI;
    private Transform tf;
    private PID xPid, yPid, zPid;
    private LineRenderer lr;

	void Start () {
        tf = transform;
        lr = GetComponent<LineRenderer>();
        xPid = new PID(p, i, d, minI, maxI);
        yPid = new PID(p, i, d, minI, maxI);
        zPid = new PID(p, i, d, minI, maxI);
	}
	
	void Update () {
        if(lr != null)
        {
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, tf.position);
        }
        Vector3 dir = (Target.position - tf.position).normalized;
        var look = Quaternion.LookRotation(dir);
        var look3 = look.eulerAngles;
        float x = xPid.Control360(Time.deltaTime, look3.x, tf.eulerAngles.x) * Time.deltaTime;
        float y = yPid.Control360(Time.deltaTime, look3.y, tf.eulerAngles.y) * Time.deltaTime;
        float z = zPid.Control360(Time.deltaTime, look3.z, tf.eulerAngles.z) * Time.deltaTime;

        Debug.DrawLine(tf.position, tf.position + dir * 10f, Color.yellow, Time.deltaTime/2f);
        Debug.DrawLine(tf.position, tf.position + tf.forward * 5f, Color.black, Time.deltaTime / 2f);

        x = Mathf.Clamp(Mathf.Abs(x) > .01f ? x : 0, -MaxRotSpeed, MaxRotSpeed);
        y = Mathf.Clamp(Mathf.Abs(y) > .01f ? y : 0, -MaxRotSpeed, MaxRotSpeed);
        z = Mathf.Clamp(Mathf.Abs(z) > .01f ? z : 0, -MaxRotSpeed, MaxRotSpeed);

        KU.Log("target : " + look3.z + " | current : " + tf.eulerAngles.z + " | z : " + z, 0, Color.blue, true, false);
        KU.Log("target : " + look3.y + " | current : " + tf.eulerAngles.y + " | z : " + y, 0, Color.green, true, false);
        KU.Log("target : " + look3.x + " | current : " + tf.eulerAngles.x + " | z : " + x, 0, Color.red, true, false);

        tf.Rotate(x, y, z);
    }
}
