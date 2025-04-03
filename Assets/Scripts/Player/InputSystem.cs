using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation due to physics
    }

    // Public method to make the player jump
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(force: Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Check if the player is grounded
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
