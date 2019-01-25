using RootMotion.Dynamics;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterPickupComponent : CharacterComponent
{
    [Header("Pick up parameters")]
    public float PickupRadius = 1f;
    public Transform PickupCheckOriginTransform;
    public bool HasObjectInHands { get { return pickedupObject != null; } }

    public PropRoot PropRootRightHand;

    public IPickable pickedupObject { get; private set;}
    private IPickable possiblePickable = null;
    private IEnumerator CheckTimer;

    private void Start()
    {
        KU.StartTimer(CheckForPickables, .1f, true);
    }

    private void CheckForPickables()
    {
        var pickables = Physics.OverlapSphere(PickupCheckOriginTransform.position, PickupRadius, LayerMask.GetMask(new string[] { "Pickable" }));
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
        if(HasObjectInHands)
        {
            Drop();
        }

        if(possiblePickable != null)
        {
            possiblePickable.Pickup();
            pickedupObject = possiblePickable;
            var weapon = ((Weapon)pickedupObject);
            PropRootRightHand.currentProp = weapon.propTemplate;
        }
    }

    public void Drop()
    {
        if(pickedupObject != null)
        {
            pickedupObject.Drop();
            pickedupObject = null;
            PropRootRightHand.DropImmediate();
        }
    }
}
