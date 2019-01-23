using UnityEngine;

public class LocalizedSoundObject : APoolObject
{
    public AudioSource Source;

    public override void ReturnToPool()
    {
        LocalizedSoundPool.Instance.Return(this);
    }

    public override void Activate()
    {
        base.Activate();
        Source.Play();
    }
}