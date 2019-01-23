using UnityEngine;

public class AssaultRifleWeapon : Weapon
{
    protected override bool Attack()
    {
        if (base.Attack())
        {
            AssaultRifleBulletPool.Instance.Get(BulletOutTransform.position, Quaternion.LookRotation(BulletOutTransform.up, -BulletOutTransform.forward), true);
            AssaultRifleMuzzlePool.Instance.Get(FXTransform.position, FXTransform.rotation, true);
            return true;
        }
        return false;
    }
}
