using UnityEngine;

public interface IPickable
{
    bool IsFree();
    Transform GetAttachTransform();
    void Pickup();
    void Drop();
    PickableType GetType();
}

public enum PickableType
{
    Weapon,
    Object,
}
