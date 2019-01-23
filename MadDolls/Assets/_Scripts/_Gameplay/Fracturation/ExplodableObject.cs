using UnityEngine;

public enum ExplosionSize
{
    Small,
    Medium,
    Big
}

public class ExplodableObject : MonoBehaviour, IExplodable
{
    public float ExplosionRadius;
    public float ExplosionForce;
    public LayerMask ExplodableLayerMask;
    public ExplosionSize SizeOfExplosion;

    private bool hasExploded = false;

    public void Explode()
    {
        if (!hasExploded || true)
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

            //Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Explode();
    }

    public void NotifyExplosion(Vector3 force)
    {
        Explode();
    }
}
