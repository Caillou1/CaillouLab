using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterPickupComponent : CharacterComponent
{
    public float PickupRadius = 1f;
    public Transform PickupCheckOriginTransform;
    public Transform PickUpHandTransform;
    public bool HasObjectInHands { get { return pickedupObject != null; } }

    public IPickable pickedupObject { get; private set;}
    private IPickable possiblePickable = null;
    private IEnumerator CheckTimer;

    private void Start()
    {
        CheckTimer = Utils.Timer(CheckForPickables, .1f, true);
        StartCoroutine(CheckTimer);
    }

    private void CheckForPickables()
    {
        var pickables = Physics.OverlapSphere(PickupCheckOriginTransform.position, PickupRadius, LayerMask.GetMask(new string[] { "Pickable" }));
        Debug.Log(pickables.Length);
        if (pickables.Length > 0) {
            bool containsWeapon = pickables.Any(c =>
            {
                var i = c.GetComponent<IPickable>();
                return i != null && i.GetType() == PickableType.Weapon;
            });
            possiblePickable = pickables.OrderBy(c => {
                float dist = float.MaxValue;
                if (!containsWeapon || (containsWeapon && c.GetComponent<IPickable>().GetType() == PickableType.Weapon))
                {
                    dist = (c.transform.position - PickupCheckOriginTransform.position).magnitude;
                }
                return dist;
            }).First().GetComponent<IPickable>();
        } else
        {
            possiblePickable = null;
        }
    }

    public void PickUp()
    {
        if(possiblePickable != null)
        {
            possiblePickable.Pickup();
            pickedupObject = possiblePickable;
            var weapon = ((Weapon)pickedupObject);
            weapon.transform.parent = PickUpHandTransform;
            Debug.Log(weapon.transform.lossyScale);
            weapon.transform.localPosition = Vector3.zero - weapon.AttachTransform.localPosition * weapon.transform.localScale.x;
        }
    }

    public void Drop()
    {
        if(pickedupObject != null)
        {
            pickedupObject.Drop();
            pickedupObject = null;
        }
    }
}
