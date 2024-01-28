// HitTracker.cs
using UnityEngine;

public class HitTracker : MonoBehaviour
{
    public int hits = 0;
    public int maxHits = 3;

    public bool IsHit()
    {
        return hits > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("poop"))
        {
            hits++;

            if (hits >= maxHits)
            {
                // Trigger game over method
                // You can replace this with your game over logic
                Debug.Log("Game Over");
                Destroy(gameObject); // Destroy the prefab when game over
            }
        }
    }
}