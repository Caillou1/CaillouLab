using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

public enum ExplosionSize
{
    Small,
    Medium,
    Big
}

public class ExplodableObject : MonoBehaviour, IExplodable
{
    public bool ExplodeOnCollision = true;
    public float ExplosionRadius;
    public float ExplosionForce;
    public LayerMask ExplodableLayerMask;
    public LayerMask PlayerLayerMask;
    public ExplosionSize SizeOfExplosion;
    public UnityEvent OnExplode;

    private bool hasExploded = false;

    public void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            var tf = transform;

            switch (SizeOfExplosion) {
                case ExplosionSize.Big:
                    CameraShake.Instance.Trauma += .5f;
                    BigExplosionPool.Instance.Get(tf.position, true);
                    break;
                case ExplosionSize.Medium:
                    CameraShake.Instance.Trauma += .33f;
                    MediumExplosionPool.Instance.Get(tf.position, true);
                    break;
                case ExplosionSize.Small:
                    CameraShake.Instance.Trauma += .15f;
                    SmallExplosionPool.Instance.Get(tf.position, true);
                    break;
            }

            var fracturableCols = Physics.OverlapSphere(tf.position, ExplosionRadius, ExplodableLayerMask);

            foreach (var col in fracturableCols)
            {
                var exp = col.GetComponent<IExplodable>();

                if (exp != null)
                {
                    var vec = col.transform.position - tf.position;
                    var dir = vec.normalized;
                    var dist = vec.magnitude;
                    float distanceFactor = Mathf.Clamp(1 - (dist / ExplosionRadius), .1f, 1f);
                    exp.NotifyExplosion(dir * ExplosionForce * distanceFactor);
                }
            }

            var players = Physics.OverlapSphere(tf.position, ExplosionRadius, PlayerLayerMask);

            foreach (var player in players)
            {
                var health = player.GetComponent<CharacterComponentHealth>();

                if (health != null)
                {
                    var vec = player.transform.position - tf.position;
                    var dir = vec.normalized;
                    var dist = vec.magnitude;
                    float distanceFactor = Mathf.Clamp(1 - (dist / ExplosionRadius), .1f, 1f);
                    var dmg = Mathf.RoundToInt(distanceFactor * ExplosionForce);
                    health.ApplyDamage(dmg, dir);
                }
            }

            if (OnExplode != null)
                OnExplode.Invoke();
        }
    }

    public void NotifyExplosion(Vector3 force)
    {
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ExplodeOnCollision && !hasExploded)
        {
            Explode();
        }
    }

    public void Reset()
    {
        hasExploded = false;
    }
}
