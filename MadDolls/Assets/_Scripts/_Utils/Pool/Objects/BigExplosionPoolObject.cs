using UnityEngine;

public class BigExplosionPoolObject : APoolObject
{
    public override void ReturnToPool()
    {
        BigExplosionPool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<ParticleSystem>().Play(true);
    }
}