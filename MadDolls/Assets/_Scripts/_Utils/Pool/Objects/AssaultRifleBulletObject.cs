using UnityEngine;

public class AssaultRifleBulletObject : APoolObject
{
    public float BulletSpeed;
    public Vector3 BulletDirection = Vector3.up;
    public Rigidbody BulletRigidbody;

    public override void ReturnToPool()
    {
        AssaultRifleBulletPool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        BulletRigidbody.velocity = BulletRigidbody.rotation * BulletDirection * BulletSpeed;
    }
}