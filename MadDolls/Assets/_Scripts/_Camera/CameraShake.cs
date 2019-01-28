using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool IsCameraShakeEnabled = true;

    public float Trauma
    {
        get
        {
            return trauma;
        }
        set
        {
            trauma = Mathf.Clamp01(value);
        }
    }

    public static CameraShake Instance { get; private set; }

    [Range(0f, 1f)] [SerializeField] private float trauma;
    [SerializeField] private float traumaMultiplier = 5f;
    [SerializeField] private float traumaMagnitude = 0.8f;
    [SerializeField] private float traumaRotationMagnitude = .5f;
    [SerializeField] private float traumaPow = .3f;
    [SerializeField] private float traumaDepthMag = 1.3f;
    [SerializeField] private float traumaDecay = 1.3f;

    private float timeCounter;
    private Transform cameraTransform;

    private void Awake()
    {
        Instance = this;
        cameraTransform = transform;
    }

    private Vector3 GetVec3()
    {
        return new Vector3(GetFloat(1), GetFloat(10), GetFloat(100) * traumaDepthMag);
    }

    private float GetFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, timeCounter) - .5f) * 2;
    }

    private void Update()
    {
        if(IsCameraShakeEnabled && Trauma > 0)
        {
            timeCounter += Time.deltaTime * Mathf.Pow(trauma, traumaPow) * traumaMultiplier;
            Vector3 newPos = GetVec3() * traumaMagnitude * Trauma;
            cameraTransform.localPosition = newPos;
            cameraTransform.localRotation = Quaternion.Euler(newPos * traumaRotationMagnitude);
            Trauma -= Time.deltaTime * traumaDecay * Trauma;
            if (Trauma < .01f) Trauma = 0;
        } else
        {
            Vector3 newPos = Vector3.Lerp(cameraTransform.localPosition, Vector3.zero, Time.deltaTime);
            cameraTransform.localPosition = newPos;
            cameraTransform.localRotation = Quaternion.Euler(newPos);
        }
    }
}
