using UnityEngine;

public class PoopScrolling : MonoBehaviour
{
    private float scrollSpeed;

    public void Initialize(float speed)
    {
        scrollSpeed = speed;
    }

    void Update()
    {
        // Move the poopy down
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime, Space.World);

        // Check if the poopy is offscreen and destroy it
        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y)
        {
            Destroy(gameObject);
        }
    }
}