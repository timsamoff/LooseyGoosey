using UnityEngine;

public class GooseController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float maxRotationAngle = 10f;

    private bool canMoveRight = true;
    private bool canMoveLeft = true;

    void Update()
    {
        Move();
        if (canMoveRight || canMoveLeft)
            Rotate();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0f) * movementSpeed * Time.deltaTime;

        // Move right only if allowed and pressing right
        if (horizontalInput > 0 && canMoveRight)
        {
            transform.Translate(movement, Space.World);
        }
        // Move left only if allowed and pressing left
        else if (horizontalInput < 0 && canMoveLeft)
        {
            transform.Translate(movement, Space.World);
        }
    }

    private void Rotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f)
        {
            float rotationAngle = -horizontalInput * maxRotationAngle;
            rotationAngle = Mathf.Clamp(rotationAngle, -maxRotationAngle, maxRotationAngle);
            transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider != null)
        {
            // Disable right movement if colliding while moving right
            if (Input.GetAxis("Horizontal") > 0)
            {
                canMoveRight = false;
            }
            // Disable left movement if colliding while moving left
            else if (Input.GetAxis("Horizontal") < 0)
            {
                canMoveLeft = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider != null)
        {
            // Enable right movement when no longer colliding and moving left
            if (Input.GetAxis("Horizontal") < 0)
            {
                canMoveRight = true;
            }
            // Enable left movement when no longer colliding and moving right
            else if (Input.GetAxis("Horizontal") > 0)
            {
                canMoveLeft = true;
            }
        }
    }
}