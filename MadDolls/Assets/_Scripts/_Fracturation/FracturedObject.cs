using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedObject : MonoBehaviour
{
    [Header("Physic")]
    public bool CanBreakOnCollision = true;
    public bool AddForceAtImpact = true;
    public float MinimumForceToBreak;

    [Header("Components")]
    public MeshRenderer NormalObjectRenderer;
    public Collider NormalObjectCollider;
    public Rigidbody NormalObjectRigidbody;
    public GameObject[] ObjectFracturedCells;

    public void Break(Vector3 force, Vector3 impactPosition)
    {
        if (force.magnitude >= MinimumForceToBreak)
        {
            NormalObjectRenderer.enabled = false;
            NormalObjectCollider.enabled = false;
            NormalObjectRigidbody.isKinematic = true;
            NormalObjectRigidbody.useGravity = false;
            foreach (var obj in ObjectFracturedCells)
            {
                obj.SetActive(true);
                if (AddForceAtImpact) obj.GetComponent<Rigidbody>().velocity = -force;
                //StartCoroutine(MoveDownCell(obj.transform, Random.Range(3, 5)));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CanBreakOnCollision)
        {
            Break(collision.relativeVelocity, Vector3.zero);
        }
    }

    IEnumerator MoveDownCell(Transform cell, float duration)
    {
        Destroy(cell.gameObject, duration);
        yield return new WaitUntil(() => { return cell == null || cell.GetComponent<Rigidbody>().velocity.magnitude <= 1; });
        if (cell != null)
        {
            cell.GetComponent<Collider>().enabled = false;
            Destroy(cell.gameObject, 1);
        }
    }
}
