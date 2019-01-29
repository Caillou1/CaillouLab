using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public List<Transform> Targets;

    public static CameraFollow Instance { get; private set; }

    [Header("Movement")]
    public Vector3 CameraOffsetDirection;
    public float CameraSmoothTime = .5f;
    [Range(0f,2f)]
    public float CameraDistanceMult = 1.25f;
    public float CameraMinDistance = 10f;

    private Camera cameraComponent;
    private Transform tf;
    private Vector3 velocity;
    private Bounds bounds;
    private Vector3 cameraOffset;
    private float cameraDistance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tf = transform;
        cameraComponent = Camera.main;
    }
    
    void LateUpdate()
    {
        if (Targets.Count == 0)
            return;

        SetBounds();
        Move();
    }

    void SetBounds()
    {
        bounds = new Bounds(Targets[0].position, Vector3.zero);

        for(int i = 1; i<Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].position);
        }
    }

    float GetGreatestDistance()
    {
        return Mathf.Sqrt(bounds.size.x * bounds.size.x + bounds.size.z * bounds.size.z);
    }

    void Move()
    {
        cameraDistance = Mathf.Max(CameraMinDistance, ((GetGreatestDistance() / 2 / cameraComponent.aspect) / Mathf.Tan(cameraComponent.fieldOfView / 2)) * CameraDistanceMult);
        cameraOffset = CameraOffsetDirection.normalized * cameraDistance;
        tf.position = Vector3.SmoothDamp(tf.position, bounds.center + cameraOffset, ref velocity, CameraSmoothTime);
    }

    public void RegisterTarget(Transform targetTransform)
    {
        if (!Targets.Contains(targetTransform))
        {
            Targets.Add(targetTransform);
        }
    }

    public void UnregisterTarget(Transform targetTransform)
    {
        if(Targets.Contains(targetTransform))
        {
            Targets.Remove(targetTransform);
        }
    }
}
