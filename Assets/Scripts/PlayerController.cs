using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Hitpoint playerhp;

    bool isPause = false;

    [Header("Base Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    Vector2 directionToMouse;

    [Header("Boost")]
    [SerializeField] float boostMax = 100f;
    [SerializeField] float boostDrainRate = 1;
    [SerializeField] float boostGainRate = .5f;
    float boostCur;
    [SerializeField] float boostPower = 10f;
    bool isBoosting;

    public delegate void OnBoostChanged(float maxvalue,float currentvalue);
    public event OnBoostChanged onBoostChanged;

    [Header("Hook")]
    [SerializeField] HookBehaviour hook;
    private bool isUsingHook = false;

    private void Start()
    {
        playerhp = GetComponent<Hitpoint>();

        boostCur = boostMax;

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
        if (isPause)
            return;

        //calculate direction toward mouse
        directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        directionToMouse.Normalize();

        // Player rotation
        RotatePlayer();

        // Apply additional force towards the mouse position when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBoosting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isBoosting = false;
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
        //BoostRegen

        //Boost
        if (isBoosting)
            ApplyAdditionalForce();
        else if (boostCur <= boostMax)
        {
            //Regen boost
            AdjustBoostGuage(boostGainRate);
        }

        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(vertical, -horizontal).normalized;
        
        MovePlayer(moveDirection);
    }

    void MovePlayer(Vector2 moveDirection)
    {
        // Apply force in the moveDirection
        Vector2 force = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.AddRelativeForce(force);
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
        if (boostCur <= 0)
            return;

        //Drain boost
        AdjustBoostGuage(-boostDrainRate);

        // Apply additional force in that direction
        rb.AddForce(directionToMouse * boostPower);
    }
    void AdjustBoostGuage(float amount)
    {
        boostCur += amount;
        onBoostChanged.Invoke(boostMax, boostCur);
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
