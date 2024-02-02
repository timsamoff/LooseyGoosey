using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings Stuff")]
    [SerializeField] private int maxPrefabsToInstantiate = 5;
    [SerializeField] private float initialSpawnDelay = 2f;
    [SerializeField] private float decreaseRate = 0.1f;
    [SerializeField] private float decreaseInterval = 15f;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float durationBeforeScreenShake = 0.2f;
    [SerializeField] private int maxMissesBeforeGameOver = 3;

    [Header("Road Boundary")]
    [SerializeField] private float leftBoundary = -1f;
    [SerializeField] private float rightBoundary = 1f;

    [Header("People Stuff")]
    [SerializeField] private GameObject[] prefabs;

    [Header("Health Stuff")]
    [SerializeField] private float healthTimer = 15f;
    [SerializeField] private AudioClip[] healthLoseSound;
    [SerializeField] private AudioClip healthRegenSound;
    [SerializeField] private Image[] healthImages;

    private float timeSinceLastHealthImageAdded = 0f;

    private AudioSource audioSource;

    private int missesCount = 0;
    private int currentHealth = 3;
    private float currentSpawnDelay;

    private GameTimer gameTimer;

    private ScreenShake screenShake;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameTimer = FindObjectOfType<GameTimer>();
        screenShake = Camera.main.GetComponent<ScreenShake>();

        currentSpawnDelay = initialSpawnDelay;
        StartCoroutine(SpawnRoutine());
        StartCoroutine(DecreaseSpawnDelay());
        UpdateHealthUI();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnPrefabs();
            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }

    private IEnumerator DecreaseSpawnDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(decreaseInterval);
            DecreaseDelay();
        }
    }

    private void DecreaseDelay()
    {
        currentSpawnDelay -= decreaseRate;
        currentSpawnDelay = Mathf.Max(currentSpawnDelay, 0.1f); // Don't go below 0.1
        Debug.Log("Spawn delay decreased to: " + currentSpawnDelay);
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

            MissTracker missTracker = instantiatedPrefabs[i].GetComponent<MissTracker>();
            if (missTracker == null)
            {
                missTracker = instantiatedPrefabs[i].AddComponent<MissTracker>();
                missTracker.objectSpawner = this;
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
                    prefabsToMove[i].transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

                    // Check if the prefab leaves the screen
                    if (prefabsToMove[i].transform.position.y < -10f)
                    {
                        // Check if the prefab was hit by a "poop" object
                        MissTracker missTracker = prefabsToMove[i].GetComponent<MissTracker>();
                        if (missTracker == null || !missTracker.isHit)
                        {
                            IncrementMisses();
                        }

                        // Destroy the prefab
                        Destroy(prefabsToMove[i]);
                    }
                }
            }

            yield return null;
        }
    }

    private void Update()
    {
        CheckGameOver();
        CheckHealthTimer();
    }

    private void CheckGameOver()
    {
        if (missesCount >= maxMissesBeforeGameOver && !audioSource.isPlaying)
        {
            Debug.Log("Game Over");

            gameTimer.EndGame();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene(2);
        }
    }

    private void CheckHealthTimer()
    {
        timeSinceLastHealthImageAdded += Time.deltaTime;

        if (timeSinceLastHealthImageAdded >= healthTimer)
        {
            timeSinceLastHealthImageAdded = 0f;

            IncrementHealthAndDecrementMisses();
        }
    }

    private void IncrementHealthAndDecrementMisses()
    {
        if (currentHealth >= 3 && missesCount <= 0)
        {
            return;
        }

        int healthImagesToAdd = Mathf.Min(1, healthImages.Length - currentHealth);
        currentHealth = Mathf.Min(currentHealth + healthImagesToAdd, 3);
        missesCount = Mathf.Max(missesCount - 1, 0);

        UpdateHealthUI();

        if (audioSource != null && healthRegenSound != null && healthImagesToAdd > 0)
        {
            audioSource.PlayOneShot(healthRegenSound);
        }

        Debug.Log("Health images added: " + healthImagesToAdd);
        Debug.Log("Misses decremented to: " + missesCount);
    }

    private void IncrementMisses()
    {
        missesCount++;

        // Clamp missesCount between 0 and 3
        missesCount = Mathf.Clamp(missesCount, 0, 3);

        currentHealth = Mathf.Max(currentHealth - 1, 0);

        Debug.Log("Misses: " + missesCount);
        UpdateHealthUI();

        if (screenShake != null)
        {
            screenShake.StartShake(durationBeforeScreenShake);
        }

        if (audioSource != null && healthLoseSound.Length > 0)
        {
            int randomIndex = Random.Range(0, healthLoseSound.Length);
            audioSource.PlayOneShot(healthLoseSound[randomIndex]);
        }
    }

    private void UpdateHealthUI()
    {
        // Update the UI images based on the remaining health
        for (int i = 0; i < healthImages.Length; i++)
        {
            bool shouldBeVisible = i < currentHealth;

            healthImages[i].enabled = shouldBeVisible;

            // If the image should not be visible, disable it
            if (!shouldBeVisible)
            {
                healthImages[i].enabled = false;
            }
        }
    }
}