using UnityEngine;

public class MissTracker : MonoBehaviour
{
    public ObjectSpawner objectSpawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("poop"))
        {
            // Notify ObjectSpawner of a hit
            Debug.Log("Prefab hit by poop!");
        }
    }
}