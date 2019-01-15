using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPickable
{
    public GameObject Bullet;
    public GameObject Case;
    public Transform BulletOutTransform;
    public Transform CaseOutTransform;
    public ParticleSystem FireParticle;
    public Transform FXTransform;
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
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
        if (isAttacking && (AttackLoop || !hasAttacked) && (Time.time - lastAttackTime) >= TimeBetweenAttack)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (ammoCount > 0 || MaxAmmunitions == -1)
        {
            hasAttacked = true;
            lastAttackTime = Time.time;
            ammoCount--;
            Instantiate(FireParticle, FXTransform.position, Quaternion.LookRotation(FXTransform.forward, FXTransform.up)).Play(true);
            Instantiate(Bullet, BulletOutTransform.position, Quaternion.LookRotation(BulletOutTransform.up, BulletOutTransform.forward));
            Instantiate(Case, CaseOutTransform.position, Quaternion.LookRotation(BulletOutTransform.up, BulletOutTransform.forward));
        }
    }

    public void Pickup()
    {
        isFree = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    PickableType IPickable.GetType()
    {
        return PickableType.Weapon;
    }

    public void Drop()
    {
        isFree = true;
    }

    public void StartUse()
    {
        OnInputStart();
    }

    public void EndUse()
    {
        OnInputStop();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}