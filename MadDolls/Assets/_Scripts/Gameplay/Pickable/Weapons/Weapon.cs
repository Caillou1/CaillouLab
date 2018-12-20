using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPickable
{
    public Transform AttachTransform;
    public bool AttackLoop;
    public bool TwoHanded;
    public float TimeBetweenAttack;
    [Min(-1)]
    public int MaxAmmunitions;
    public int DamagePerHit;

    protected bool isFree = true;
    protected bool isAttacking = false;
    protected bool hasAttacked = false;
    protected float lastAttackTime;
    protected int ammoCount;
    protected Transform weaponTransform;

    private void Start()
    {
        ammoCount = MaxAmmunitions;
        weaponTransform = transform;
    }

    public virtual Transform GetAttachTransform()
    {
        return AttachTransform;
    }

    public bool IsFree()
    {
        return isFree;
    }

    public virtual void OnInputStart()
    {
        if (ammoCount > 0 || MaxAmmunitions == -1)
        {
            isAttacking = true;
        }
    }

    public virtual void OnInputStop()
    {
        isAttacking = false;
        hasAttacked = false;
    }

    private void Update()
    {
        if(isAttacking && (AttackLoop || !hasAttacked) && (Time.time - lastAttackTime) >= TimeBetweenAttack)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        hasAttacked = true;
        lastAttackTime = Time.time;
        if(ammoCount > 0)
            ammoCount--;
    }

    public void Pickup()
    {
        isFree = false;
    }

    PickableType IPickable.GetType()
    {
        return PickableType.Weapon;
    }

    public void Drop()
    {
        isFree = true;
    }
}