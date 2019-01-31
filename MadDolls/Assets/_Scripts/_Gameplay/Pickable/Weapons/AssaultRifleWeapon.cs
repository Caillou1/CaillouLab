using UnityEngine;

public class AssaultRifleWeapon : Weapon
{
    protected override void Attack()
    {
        base.Attack();
        AssaultRifleBulletPool.Instance.Get(BulletOutTransform.position, Quaternion.LookRotation(Quaternion.Euler(Random.Range(-2,2f),Random.Range(-2f, 2f), Random.Range(-2f, 2f)) * BulletOutTransform.up, -BulletOutTransform.forward), true);
        AssaultRifleMuzzlePool.Instance.Get(FXTransform.position, FXTransform.rotation, true);
    }
}
