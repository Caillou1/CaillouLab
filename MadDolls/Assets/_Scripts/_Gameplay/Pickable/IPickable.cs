using UnityEngine;

public interface IPickable
{
    bool IsFree();
    void Pickup();
    void Drop();
    Transform GetTransform();

    void StartUse();
    void EndUse();
    PickableType GetType();
    
}

public enum PickableType
{
    Weapon,
    Object,
}
