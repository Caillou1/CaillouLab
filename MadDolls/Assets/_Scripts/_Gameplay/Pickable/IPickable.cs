using UnityEngine;

public interface IPickable
{
    bool IsFree();
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
