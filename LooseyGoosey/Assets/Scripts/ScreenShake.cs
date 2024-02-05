using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Vector3 originalCameraPosition;
    [SerializeField] private float shakeDuration = 0.0f;
    [SerializeField] private float shakeMagnitude = 0.7f;

    void Awake()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            mainCameraTransform.localPosition = originalCameraPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            // mainCameraTransform.localPosition = originalCameraPosition;
        }
    }

    public void StartShake(float duration)
    {
        originalCameraPosition = mainCameraTransform.localPosition;
        shakeDuration = duration;
    }
}