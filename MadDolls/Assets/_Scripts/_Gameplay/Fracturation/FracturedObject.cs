using System.Collections;
using UnityEngine;

public class FracturedObject : MonoBehaviour, IExplodable
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

    public void Break(Vector3 force)
    {
        Debug.Log(force.magnitude);
        if (force.magnitude >= MinimumForceToBreak)
        {
            NormalObjectRenderer.enabled = false;
            NormalObjectCollider.enabled = false;
            NormalObjectRigidbody.isKinematic = true;
            NormalObjectRigidbody.useGravity = false;
            foreach (var obj in ObjectFracturedCells)
            {
                obj.SetActive(true);
                if (AddForceAtImpact) obj.GetComponent<Rigidbody>().velocity = force;
                StartCoroutine(MoveDownCell(obj.transform, Random.Range(3, 5)));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CanBreakOnCollision)
        {
            Break(-collision.relativeVelocity);
        }
    }

    IEnumerator MoveDownCell(Transform cell, float duration)
    {
        yield return new WaitUntil(() => { return cell == null || cell.GetComponent<Rigidbody>().velocity.magnitude <= .25f; });
        yield return new WaitForSeconds(duration);
        if (cell != null)
        {
            cell.GetComponent<Collider>().enabled = false;
            Destroy(cell.gameObject, 1);
        }
    }

    public void NotifyExplosion(Vector3 force)
    {
        Break(force);
    }
}
