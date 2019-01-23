using UnityEngine;

public class AssaultRifleMuzzleObject : APoolObject
{
    public override void ReturnToPool()
    {
        AssaultRifleMuzzlePool.Instance.Return(this);
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