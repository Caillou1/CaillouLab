using UnityEngine;

public interface IPickable
{
    bool IsFree();
    Transform GetAttachTransform();
    void Pickup();
    void Drop();

    void StartUse();
    void EndUse();
    PickableType GetType();
    
}

public enum PickableType
{
    Weapon,
    Object,
}
