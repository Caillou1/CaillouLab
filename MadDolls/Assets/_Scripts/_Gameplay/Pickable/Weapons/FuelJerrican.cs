using UnityEngine;

public class FuelJerrican : MonoBehaviour {
    public float MinimumDistanceBetweenSplatters = .1f;

    private Vector3 lastPosition = Vector3.negativeInfinity;
    private void Awake() {
        GetComponentInChildren<FuelPouringParticleSystem>().Register(this);
    }

    public void NotifySplatter(Vector3 position)
    {
        if(Vector3.Distance(lastPosition, position) >= MinimumDistanceBetweenSplatters)
        {
            FuelSplatterPool.Instance.Get(position, Quaternion.identity, true);
            lastPosition = position;
        }
    }
}