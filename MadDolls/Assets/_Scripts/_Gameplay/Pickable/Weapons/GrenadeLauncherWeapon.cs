using UnityEngine;

public class GrenadeLauncherWeapon : Weapon
{
    protected override void Attack()
    {
        base.Attack();
        GrenadeLauncherBulletPool.Instance.Get(BulletOutTransform.position, Quaternion.LookRotation(Quaternion.Euler(Random.value,Random.value,Random.value) * BulletOutTransform.up, -BulletOutTransform.forward), true);
        //GrenadeLauncherMuzzlePool.Instance.Get(FXTransform.position, FXTransform.rotation, true);
    }
}
