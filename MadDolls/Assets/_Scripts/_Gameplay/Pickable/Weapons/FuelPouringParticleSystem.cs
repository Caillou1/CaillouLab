using System.Collections.Generic;
using UnityEngine;

public class FuelPouringParticleSystem : MonoBehaviour
{
    public ParticleSystem PouringPS;

    private FuelJerrican jerrican;
    public void Register(FuelJerrican j)
    {
        jerrican = j;
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        PouringPS.GetCollisionEvents(other, collisionEvents);
        foreach (var colEvent in collisionEvents)
        {
            jerrican.NotifySplatter(colEvent.intersection);
        }
    }
}