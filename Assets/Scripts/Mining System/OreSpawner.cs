using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrePile : MonoBehaviour
{
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private float oresAmount = 4f;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float parabolicHeight = 1.5f;
    [SerializeField] private float movementDuration = 2f;

    [SerializeField] private float spawnAreaWidth = 3f;
    [SerializeField] private float spawnAreaLength = 5f;

    [SerializeField] private CameraShake cameraShake;

    private Coroutine spawnCoroutine;
    private float spawnedOres = 0f;

    private void Start()
    {
        // Start the coroutine when the player raycast is interacting
        Laser.OnInteract += StartCubeSpawning;
        // Stop the coroutine when the player raycast stops interacting
        Laser.OnStopInteract += StopCubeSpawning;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events to avoid memory leaks
        Laser.OnInteract -= StartCubeSpawning;
        Laser.OnStopInteract -= StopCubeSpawning;
    }

    private void StartCubeSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnOre());
        }
    }

    private void StopCubeSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnOre()
    {
        while (spawnedOres <= oresAmount)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 randomPosition = new Vector3(
                transform.position.x + Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
                transform.position.y,
                transform.position.z + Random.Range(-spawnAreaLength / 2f, spawnAreaLength / 2f)
            );

            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity);
            ore.GetComponent<Rigidbody>().sleepThreshold = 0.01f;

            spawnedOres++;

            cameraShake.Shake();

            StartCoroutine(MoveOre(ore, randomPosition));

            if (spawnedOres >= oresAmount)
            {
                StopCubeSpawning();
                Destroy(this);
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator MoveOre(GameObject ore, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = ore.transform.position;

        while (elapsedTime < movementDuration)
        {
            float t = elapsedTime / movementDuration;
            float height = Mathf.Sin(t * Mathf.PI) * parabolicHeight;
            ore.transform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * height;

            elapsedTime += Time.deltaTime;
            Rigidbody oreRigidbody = ore.GetComponent<Rigidbody>();
            oreRigidbody.MovePosition(ore.transform.position);

            yield return null;
        }
    }
}
