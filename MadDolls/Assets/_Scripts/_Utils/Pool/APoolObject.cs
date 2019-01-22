using UnityEngine;


public abstract class APoolObject : MonoBehaviour
{
    public float Duration;

    private Transform tf;

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
        StartCoroutine(Utils.Timer(ReturnToPool, Duration, false));
    }

    public void SetPosition(Vector3 pos)
    {
        tf.position = pos;
    }

    public abstract void ReturnToPool();
}
