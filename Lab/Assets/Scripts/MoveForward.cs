using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {
    public float Speed = 50f;
    public float RotationSpeed = 5f;
    public float TimeBeforeRotation;
    public bool RotateRandom = false;

    private Transform tf;
    [SerializeField]
    private float speed;
    private int inputX, inputY;

	void Start () {
        tf = transform;
        if(RotateRandom)
        {
            StartCoroutine(RandomInput());
        }
        speed = Speed;
    }
	
	void Update () {
        tf.Translate(Vector3.forward * Time.deltaTime * speed);
        
        tf.Rotate(RotationSpeed * Time.deltaTime * inputX, RotationSpeed * Time.deltaTime * inputY, 0);
	}

    IEnumerator RandomInput()
    {
        while(true)
        {
            inputX = Random.Range(-3, 4);
            inputY = Random.Range(-3, 4);
            speed = Random.Range(Speed, Speed + 10f);
            yield return new WaitForSeconds(TimeBeforeRotation);
        }
    }
}
