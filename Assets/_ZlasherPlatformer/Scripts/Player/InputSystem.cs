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

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool GetJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetAttack()
    {
        return Input.GetMouseButtonDown(0);
    }

    public float GetHorizontal()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            return -1f;
        }
        return 0f;
    }

    public bool GetHorizontalReleased()
    {
        return Input.GetKeyUp(KeyCode.D);
    }

    public bool GetMoveLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    public bool GetMoveRight()
    {
        return Input.GetKey(KeyCode.D);
    }
}
