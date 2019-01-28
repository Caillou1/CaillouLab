﻿using UnityEngine;

public class AssaultRifleWeapon : Weapon
{
    protected override void Attack()
    {
        base.Attack();
        AssaultRifleBulletPool.Instance.Get(BulletOutTransform.position, Quaternion.LookRotation(Quaternion.Euler(Random.value,Random.value,Random.value) * BulletOutTransform.up, -BulletOutTransform.forward), true);
        AssaultRifleMuzzlePool.Instance.Get(FXTransform.position, FXTransform.rotation, true);
        Rewired.ReInput.players.GetPlayer(2).controllers.Joysticks[0].SetVibration(1,1,.25f,.25f);
    }
}
