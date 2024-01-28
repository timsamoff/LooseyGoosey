using UnityEngine;

public class PoopCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("object"))
        {
            Debug.Log("Poop touched the object!");
        }
    }
}