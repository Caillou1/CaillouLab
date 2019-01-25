using System.Collections;
using UnityEngine;


public abstract class APoolObject : MonoBehaviour
{
    public float Duration;

    private Transform tf;
    private IEnumerator timer;

    private void Awake()
    {
        tf = transform;
    }

    public virtual void Reset()
    {
        gameObject.SetActive(false);
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        timer = KU.StartTimer(ReturnToPool, Duration);
    }

    public void SetPosition(Vector3 pos)
    {
        tf.position = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        tf.rotation = rot;
    }

    public abstract void ReturnToPool();

    public virtual void ReturnImmediate()
    {
        KU.StopTimer(timer);
        ReturnToPool();
    }
}
