using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxPrefabsToInstantiate = 5;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float leftBoundary = -1f;
    [SerializeField] private float rightBoundary = 1f;
    [SerializeField] private int maxHitsBeforeGameOver = 3;
    [SerializeField] private int maxMissesBeforeGameOver = 3;

    private int missesCount = 0; // Counter for misses

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnPrefabs();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnPrefabs()
    {
        int numPrefabsToInstantiate = Random.Range(1, maxPrefabsToInstantiate + 1);

        GameObject[] instantiatedPrefabs = new GameObject[numPrefabsToInstantiate];

        for (int i = 0; i < numPrefabsToInstantiate; i++)
        {
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
            float randomX = Random.Range(leftBoundary, rightBoundary);
            float randomY = Random.Range(5f, 10f);

            instantiatedPrefabs[i] = Instantiate(randomPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
            instantiatedPrefabs[i].name = "Object";

            // Attach the HitTracker script if not already attached
            HitTracker hitTracker = instantiatedPrefabs[i].GetComponent<HitTracker>();
            if (hitTracker == null)
            {
                hitTracker = instantiatedPrefabs[i].AddComponent<HitTracker>();
                hitTracker.maxHits = maxHitsBeforeGameOver;
            }
        }

        StartCoroutine(MovePrefabs(instantiatedPrefabs));
    }

    private IEnumerator MovePrefabs(GameObject[] prefabsToMove)
    {
        while (true)
        {
            for (int i = 0; i < prefabsToMove.Length; i++)
            {
                if (prefabsToMove[i] != null)
                {
                    prefabsToMove[i].transform.Translate(Vector3.down * speed * Time.deltaTime);

                    // Destroy prefabs when they leave the screen
                    if (prefabsToMove[i].transform.position.y < -10f)
                    {
                        // Check if the prefab is not hit by "poop" before incrementing misses
                        if (!prefabsToMove[i].GetComponent<HitTracker>().IsHit())
                        {
                            // Increase the misses count and display it in the console
                            missesCount++;
                            Debug.Log("Misses: " + missesCount);

                            CheckGameOver();
                        }

                        Destroy(prefabsToMove[i]);
                    }
                }
            }

            yield return null;
        }
    }

    private void CheckGameOver()
    {
        if (missesCount >= maxMissesBeforeGameOver)
        {
            // Game Over
            Debug.Log("Game Over");
        }
    }
}