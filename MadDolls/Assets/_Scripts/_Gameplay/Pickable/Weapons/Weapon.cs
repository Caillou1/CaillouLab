using RootMotion.Dynamics;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPickable
{
    [Header("Objects")]
    public GameObject Bullet;
    public GameObject Case;
    [Header("Transforms")]
    public Transform BulletOutTransform;
    public Transform CaseOutTransform;
    public Transform FXTransform;

    [Header("Weapon Parameters")]
    public bool AttackLoop;
    public bool TwoHanded;
    public float TimeBetweenAttack;
    [Min(-1)]
    public int MaxAmmunitions;
    public int DamagePerHit;
    public PropTemplate propTemplate;
    protected bool isFree = true;
    protected bool isAttacking = false;
    protected bool hasAttacked = false;
    protected float lastAttackTime;
    protected int ammoCount;
    protected Transform weaponTransform;
    public AudioClip WeaponSound;

    protected bool CanAttack
    {
        get
        {
            return (isAttacking && (AttackLoop || !hasAttacked) && (ammoCount > 0 || MaxAmmunitions == -1) && (Time.time - lastAttackTime) >= TimeBetweenAttack);
        }
    }

    private void Start()
    {
        ammoCount = MaxAmmunitions;
        weaponTransform = transform;
    }

    public bool IsFree()
    {
        return isFree;
    }

    private void Update()
    {
        if (CanAttack)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        hasAttacked = true;
        lastAttackTime = Time.time;
        ammoCount--;
        PlaySound();
    }

    protected virtual void PlaySound()
    {
        var sound = LocalizedSoundPool.Instance.Get(weaponTransform.position);
        sound.Duration = WeaponSound.length;
        sound.Source.clip = WeaponSound;
        sound.Source.pitch = Random.Range(.95f, 1.05f);
        sound.Activate();
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

    public virtual void StartUse()
    {
        if (ammoCount > 0 || MaxAmmunitions == -1)
        {
            isAttacking = true;
        }
    }

    public virtual void EndUse()
    {
        isAttacking = false;
        hasAttacked = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}