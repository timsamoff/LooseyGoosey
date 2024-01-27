using System.Collections;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float scrollDuration = 2f;
    [SerializeField] private float delayBetweenScrolls = 1f;
    [SerializeField] private float noSpawnZoneLeft = 200f;
    [SerializeField] private float noSpawnZoneRight = 400f;

    void Start()
    {
        StartCoroutine(ScrollAndInstantiate());
    }

    IEnumerator ScrollAndInstantiate()
    {
        while (true)
        {
            // Instantiate prefabs at random on-screen positions
            foreach (GameObject prefab in prefabs)
            {
                Vector3 randomOnScreenPosition = GetRandomOnScreenPosition();
                Instantiate(prefab, randomOnScreenPosition, Quaternion.identity);
            }

            // Scroll prefabs from on-screen to off-screen
            foreach (GameObject prefab in prefabs)
            {
                yield return StartCoroutine(ScrollPrefab(prefab));
            }

            // Delay between scrolls
            yield return new WaitForSeconds(delayBetweenScrolls);
        }
    }

    IEnumerator ScrollPrefab(GameObject obj)
    {
        float elapsedTime = 0f;

        while (elapsedTime < scrollDuration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / scrollDuration;

            // Lerp between on-screen and off-screen positions
            obj.transform.position = Vector3.Lerp(GetRandomOnScreenPosition(), GetRandomOffScreenPosition(), t);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

    Vector3 GetRandomOnScreenPosition()
    {
        float x;
        float y;

        do
        {
            x = Random.Range(0f, Screen.width);
            y = Random.Range(0f, Screen.height);
        } while (IsWithinNoSpawnZone(x));

        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f));
    }

    Vector3 GetRandomOffScreenPosition()
    {
        float x = Random.Range(0f, Screen.width);
        float y = Random.Range(Screen.height, Screen.height + 5f); // Adjust the range based on how far off-screen you want them to spawn
        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f));
    }

    bool IsWithinNoSpawnZone(float xScreenPosition)
    {
        float xWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xScreenPosition, 0f, 10f)).x;

        return xWorldPosition > noSpawnZoneLeft && xWorldPosition < noSpawnZoneRight;
    }
}