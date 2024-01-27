using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;  // The road
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private int numberOfPrefabs = 3;  // Number of road prefabs needed to cover the screen height 

    private float screenHeight;
    private List<GameObject> roadPrefabs = new List<GameObject>();

    void Start()
    {
        // Screen height
        screenHeight = Camera.main.orthographicSize * 2f;

        // Instantiate road prefabs to cover the height of the screen
        InstantiateRoadPrefabs();

        // Add a single road prefab at the bottom during initialization
        AddBottomRoadPrefabOnce();
    }

    void Update()
    {
        // Move road prefabs down
        foreach (GameObject road in roadPrefabs)
        {
            road.transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);
        }

        // Check if the top road prefab needs to be added
        if (roadPrefabs[roadPrefabs.Count - 1].transform.position.y > screenHeight)
        {
            // Only add one new road prefab at a time
            if (roadPrefabs.Count == 3)
            {
                AddTopRoadPrefab();
            }
        }

        // Destroy prefabs when they leave the screen
        if (roadPrefabs.Count > numberOfPrefabs && roadPrefabs[0].transform.position.y < -screenHeight)
        {
            Destroy(roadPrefabs[0]);
            roadPrefabs.RemoveAt(0);

            // Add a new prefab to the top when the last prefab is destroyed
            AddTopRoadPrefab();
        }
    }

    void InstantiateRoadPrefabs()
    {
        // Instantiate road prefabs to cover the height of the screen
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            GameObject newRoad = Instantiate(roadPrefab, new Vector2(transform.position.x, i * GetRoadHeight()), Quaternion.identity);
            roadPrefabs.Add(newRoad);
        }
    }

    void AddTopRoadPrefab()
    {
        float offsetY = GetRoadHeight();

        // Instantiate a new road prefab at the top
        GameObject newRoad = Instantiate(roadPrefab, new Vector2(transform.position.x, roadPrefabs[roadPrefabs.Count - 1].transform.position.y + offsetY), Quaternion.identity);
        roadPrefabs.Add(newRoad);
    }

    void AddBottomRoadPrefabOnce()
    {
        float offsetY = GetRoadHeight();

        // Instantiate a single road prefab at the bottom during initialization
        GameObject bottomRoad = Instantiate(roadPrefab, new Vector2(transform.position.x, roadPrefabs[0].transform.position.y - offsetY), Quaternion.identity);
        roadPrefabs.Insert(0, bottomRoad);  // Insert at the beginning of the list to maintain order
    }

    float GetRoadHeight()
    {
        // Get the height of the road prefab
        return roadPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
    }
}