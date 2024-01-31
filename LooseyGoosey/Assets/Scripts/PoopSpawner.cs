using UnityEngine;

public class PoopSpawner : MonoBehaviour
{
    [Header("Poop Settings")]
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private int numberOfSplats = 3;
    [SerializeField] private float splatInterval = 0.1f;
    [SerializeField] private float scrollSpeed = 5.0f;
    [SerializeField] private AudioClip[] poopSounds;

    private AudioSource audioSource;
    private bool isSpawning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpawning)
        {
            StartCoroutine(SpawnSplatEffect());
        }
    }

    System.Collections.IEnumerator SpawnSplatEffect()
    {
        isSpawning = true;

        for (int i = 0; i < numberOfSplats; i++)
        {
            InstantiatePoop();
            PlayRandomPoopSound();
            yield return new WaitForSeconds(splatInterval);
        }

        isSpawning = false;
    }

    void InstantiatePoop()
    {
        // Instantiate the poopy
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.extents.y, transform.position.z);
        GameObject newPoop = Instantiate(poopPrefab, spawnPosition, Quaternion.identity);

        // Detach poopy
        newPoop.transform.parent = null;

        // Attach the scrolling script to the poopy
        newPoop.AddComponent<PoopScrolling>().Initialize(scrollSpeed);

        newPoop.name = "Poop";
    }

    void PlayRandomPoopSound()
    {
        if (poopSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, poopSounds.Length);
            audioSource.PlayOneShot(poopSounds[randomIndex]);
        }
    }
}