using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect

    private Vector3 originalPosition; // Original position of the camera
    private Quaternion originalRotation; // Original rotation of the camera

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate random displacements
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetZ = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply the displacements to the camera's position
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);

            // Generate random rotations
            float angleX = Random.Range(-1f, 1f) * shakeMagnitude;
            float angleY = Random.Range(-1f, 1f) * shakeMagnitude;
            float angleZ = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply the rotations to the camera's rotation
            transform.localRotation = originalRotation * Quaternion.Euler(angleX, angleY, angleZ);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset the camera's position and rotation
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
