using UnityEngine;

public class FuelSplatterPoolObject : APoolObject
{
    public bool ToDelete = true;
    public override void ReturnToPool()
    {
        FuelSplatterPool.Instance.Return(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(this.tag))
        {
            var c = other.transform.GetComponent<FuelSplatterPoolObject>();
            if (c.ToDelete)
            {
                c.ToDelete = false;
                ReturnToPool();
            }
        }
    }
}