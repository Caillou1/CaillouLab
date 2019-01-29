using UnityEngine;

public class GrenadeLauncherMuzzleObject : APoolObject
{
    public float BulletSpeed;
    public Vector3 BulletDirection = Vector3.up;
    public Rigidbody BulletRigidbody;
    public TrailRenderer TrailRend;

    public override void ReturnToPool()
    {
        GrenadeLauncherMuzzlePool.Instance.Return(this);
    }

    public override void Reset()
    {
        base.Reset();
        BulletRigidbody.velocity = Vector3.zero;
        TrailRend.Clear();
    }

    public override void Activate()
    {
        base.Activate();
        BulletRigidbody.velocity = BulletRigidbody.rotation * BulletDirection * BulletSpeed;
    }
}