using UnityEngine;

public class SmallExplosionPoolObject : APoolObject
{
    public override void ReturnToPool()
    {
        SmallExplosionPool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<ParticleSystem>().Play(true);
    }
}