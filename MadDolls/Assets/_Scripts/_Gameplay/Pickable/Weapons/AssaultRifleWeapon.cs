public class AssaultRifleWeapon : Weapon
{
    protected override bool Attack()
    {
        if (base.Attack())
        {
            AssaultRifleMuzzlePool.Instance.Get(FXTransform.position, FXTransform.rotation, true);
            return true;
        }
        return false;
    }
}
