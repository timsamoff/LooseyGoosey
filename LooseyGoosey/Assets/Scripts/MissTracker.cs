using UnityEngine;

public class MissTracker : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    public bool isHit = false; // Flag to indicate if the prefab is hit

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("poop"))
        {
            isHit = true;
            // Notify ObjectSpawner of a hit
            Debug.Log("Prefab hit by poop!");
        }
    }
}