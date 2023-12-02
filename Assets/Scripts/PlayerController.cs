using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float additionalForce = 10f; // Adjust the value based on your preference
    public float hookSpeed = 10f; // Adjust the value based on your preference
    public float hookDelay = 1.5f; // Adjust the delay time based on your preference

    private Rigidbody2D rb;
    private bool isUsingHook = false;
    private Coroutine hookCoroutine;
    private Vector2 hookTargetPosition; // Store the target position during the hook

    private void Start()
    {
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
    }

    private void Update()
    {
        // Player rotation
        RotatePlayer();

        // Apply additional force towards the mouse position when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isUsingHook)
            {
                // If the hook is already active, cancel it
                StopCoroutine(hookCoroutine);
                isUsingHook = false;
            }
            else
            {
                ApplyAdditionalForce();

                // Start the hook delay after the hook is used
                StartCoroutine(StartHookDelay());
            }
        }

        // Hook mechanic: Move towards the mouse position quickly when clicking after a delay
        if (Input.GetMouseButtonDown(0))
        {
            if (!isUsingHook)
            {
                hookCoroutine = StartCoroutine(DelayedApplyForceToMousePosition());
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

    IEnumerator DelayedApplyForceToMousePosition()
    {
        yield return null;

        isUsingHook = true;
        hookTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate direction towards the mouse position
        Vector2 directionToMouse = (hookTargetPosition - (Vector2)transform.position).normalized;

        // Apply force in that direction
        rb.AddForce(directionToMouse * hookSpeed, ForceMode2D.Impulse);

        isUsingHook = false; // Reset the flag when the coroutine completes
    }

    IEnumerator StartHookDelay()
    {
        yield return new WaitForSeconds(hookDelay);
        // Implement anything you want to do after the hook delay here
        Debug.Log("Hook delay completed!");
    }
}
