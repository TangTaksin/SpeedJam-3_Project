using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Hitpoint playerhp;
    
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float additionalForce = 10f; // Adjust the value based on your preference

    [SerializeField] HookBehaviour hook;
    private bool isUsingHook = false;

    private void Start()
    {
        playerhp = GetComponent<Hitpoint>();

        // Ensure the object has a Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            // Add a Rigidbody2D component if it doesn't exist
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Make sure the Rigidbody2D has the appropriate settings for a zero-gravity environment
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (hook != null)
            hook.onHookFinished += HookReturn;
    }

    private void Update()
    {
        // Player rotation
        RotatePlayer();

        // Apply additional force towards the mouse position when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyAdditionalForce();
        }

        // Hook mechanic: Move towards the mouse position quickly when clicking after a delay
        if (Input.GetMouseButtonDown(0))
        {
            //if (!isUsingHook)
            {
                ShootHook();
            }
        }
    }

    private void FixedUpdate()
    {
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        MovePlayer(moveDirection);
    }

    void MovePlayer(Vector2 moveDirection)
    {
        // Apply force in the moveDirection
        Vector2 force = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.AddForce(force);
    }

    void RotatePlayer()
    {
        // Rotate the player based on input
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void ApplyAdditionalForce()
    {
        // Calculate direction towards the mouse position
        Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // Apply additional force in that direction
        rb.AddForce(directionToMouse * additionalForce, ForceMode2D.Impulse);
    }


    void ShootHook()
    {
        hook?.Shot();
        isUsingHook = true;
    }
    void HookReturn()
    {
        isUsingHook = false;
    }
}
