using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Hitpoint playerhp;
    private AudioManager audioManager;

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

    public delegate void OnBoostChanged(float maxvalue, float currentvalue);
    public event OnBoostChanged onBoostChanged;

    [Header("Hook")]
    [SerializeField] HookBehaviour hook;

    [Header("Attack")]
    bool canAttack;
    [SerializeField] float attackTime = 3f;
    [SerializeField] float knockbackForce = 10f;
    float attackTimer;
    [SerializeField] Animator TackleAnim;

    public delegate void OnHealthZero();
    public static event OnHealthZero onHealthZero;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        // playerhp = GetComponent<Hitpoint>();

        boostCur = boostMax;

        // Ensure the object has a Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found. Adding dynamically.");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Make sure the Rigidbody2D has the appropriate settings for a zero-gravity environment
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (hook != null)
        {
            hook.onHookStateChanged += onHookStateChanged;
        }

        playerhp = GetComponent<Hitpoint>();
        if (playerhp != null)
        {
            playerhp.onHealthChanged += onHealthChanged;
        }
    }

    private void Update()
    {
        if (isPause)
            return;

        AttackTimerUpdate();

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
            audioManager?.PlaySFX(1);
            ShootHook();

        }
    }

    private void FixedUpdate()
    {
        // Boost
        if (isBoosting)
            ApplyAdditionalForce();
        else if (boostCur <= boostMax)
        {
            // Regen boost
            AdjustBoostGuage(boostGainRate);
        }

        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

        MovePlayer(moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D enemybody);
        //apply knockback if possible
        var deflectDirection = collision.transform.position - transform.position;
        deflectDirection.Normalize();

        enemybody.AddForce(deflectDirection * knockbackForce, ForceMode2D.Impulse);

        if (canAttack)
        {
            collision.gameObject.TryGetComponent<Hitpoint>(out Hitpoint enemyHp);
            enemyHp.ReduceHP(1);
        }
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
        if (boostCur <= 0)
            return;

        // Drain boost
        AdjustBoostGuage(-boostDrainRate);

        // Apply additional force in that direction
        rb.AddForce(directionToMouse * boostPower);

        audioManager?.PlaySFX(0);
    }

    void AdjustBoostGuage(float amount)
    {
        boostCur += amount;
        onBoostChanged?.Invoke(boostMax, boostCur);
    }

    void ShootHook()
    {
        hook?.Shot();
    }

    void onHookStateChanged(HookBehaviour.HookState state)
    {
        switch (state)
        {
            case HookBehaviour.HookState.Idle:
                boostCur += boostMax * 0.2f;
                break;

            case HookBehaviour.HookState.Out:
                break;

            case HookBehaviour.HookState.Pull:
                EnterAttackState();
                break;

            case HookBehaviour.HookState.Return:
                audioManager?.PlaySFX(3);
                break;
        }
    }

    void onHealthChanged(int health, int maxHitPoint)
    {
        Debug.Log($"Health: {health}, Max Hitpoint: {maxHitPoint}");

        //game over condit.
        if (health <= 0)
        {

            Debug.Log("Game Over!");
            onHealthZero?.Invoke();
        }
    }

    void EnterAttackState()
    {
        attackTimer = attackTime;

        playerhp?.SetIsInvulnerable(true);
        TackleAnim.gameObject?.SetActive(true);

        canAttack = true;
    }

    void AttackTimerUpdate()
    {
        if (canAttack)
        {
            attackTimer -= Time.deltaTime;

            //Exit attack state if time ran out
            if (attackTimer <= 0)
            {
                canAttack = false;
                playerhp?.SetIsInvulnerable(false);
                TackleAnim.gameObject?.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (hook != null)
        {
            hook.onHookStateChanged -= onHookStateChanged;
        }

        if (playerhp != null)
        {
            playerhp.onHealthChanged -= onHealthChanged;
        }
    }
}