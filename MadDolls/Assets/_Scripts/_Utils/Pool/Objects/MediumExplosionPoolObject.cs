using UnityEngine;

public class MediumExplosionPoolObject : APoolObject
{
    public override void ReturnToPool()
    {
        MediumExplosionPool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<ParticleSystem>().Play(true);
    }
}