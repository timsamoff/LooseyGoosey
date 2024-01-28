using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("poop"))
        {
            Debug.Log("Object touched the poop!");
        }
    }
}