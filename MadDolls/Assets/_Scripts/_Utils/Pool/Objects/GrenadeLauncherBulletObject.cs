using UnityEngine;

public class GrenadeLauncherBulletObject : APoolObject
{
    public float BulletSpeed;
    public Vector3 BulletDirection = Vector3.up;
    public Rigidbody BulletRigidbody;
    public TrailRenderer TrailRend;

    public override void ReturnToPool()
    {
        GrenadeLauncherBulletPool.Instance.Return(this);
    }

    public override void Reset()
    {
        base.Reset();
        BulletRigidbody.velocity = Vector3.zero;
        TrailRend.Clear();
        GetComponent<ExplodableObject>().Reset();
    }

    public override void Activate()
    {
        base.Activate();
        BulletRigidbody.velocity = BulletRigidbody.rotation * BulletDirection * BulletSpeed;
    }
}