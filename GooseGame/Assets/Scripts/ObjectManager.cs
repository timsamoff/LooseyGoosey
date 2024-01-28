using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private int maxMisses = 3;

    private int misses = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("poop"))
        {
        }
        else if (other.CompareTag("object"))
        {
            misses++;
            if (misses >= maxMisses)
            {
                Debug.Log("Game Over");
            }
        }
    }
}