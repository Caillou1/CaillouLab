using UnityEngine;

public class ShotgunMuzzleObject : APoolObject
{
    public override void ReturnToPool()
    {
        ShotgunMuzzlePool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        foreach (var ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play(true);
        }
    }
}