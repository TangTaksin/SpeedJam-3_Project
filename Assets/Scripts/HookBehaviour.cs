using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    [SerializeField] float hookSpeed = 5f;
    [SerializeField] float hookRange = 5f;
    [SerializeField] float hookReturnRange = 1f;
    [SerializeField] float pullPower = 1f;
    [SerializeField] float rotationHookSpeed = 2f;
    [SerializeField] float returnForce = 1f;
    [SerializeField] LayerMask grabableLayer;

    Rigidbody2D player;
    Rigidbody2D hook;
    LineRenderer hookLine;

    Vector2 hookTargetPosition;
    Vector2 distanceFromPlayer;
    Vector2 distanceFromTarget;

    public enum HookState { Out, Pull, Return, Idle }
    HookState currentHookState = HookState.Idle;

    public delegate void OnHookStateChanged(HookState state);
    public event OnHookStateChanged onHookStateChanged;

    private void Start()
    {
        player = transform.parent.GetComponent<Rigidbody2D>();

        hook = GetComponent<Rigidbody2D>();
        hook.bodyType = RigidbodyType2D.Kinematic;

        hookLine = GetComponent<LineRenderer>();
    }

    public void Shot()
    {
        if (currentHookState == HookState.Out || currentHookState == HookState.Pull)
        {
            currentHookState = HookState.Return;
            onHookStateChanged?.Invoke(currentHookState);
        }

        if (currentHookState == HookState.Idle)
        {
            transform.parent = null;

            currentHookState = HookState.Out;
            onHookStateChanged?.Invoke(currentHookState);

            hookTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = player.position + (hookTargetPosition - player.position).normalized;

            var directionToMouse = (hookTargetPosition - (Vector2)transform.position).normalized;

            hook.bodyType = RigidbodyType2D.Dynamic;
            hook.AddForce(directionToMouse * hookSpeed, ForceMode2D.Impulse);
        }
    }

    void Pull()
    {
        hook.velocity = Vector2.zero;
        player.AddForce(-distanceFromPlayer * pullPower);

        if (distanceFromPlayer.magnitude < hookReturnRange)
        {
            Disable();
        }
    }

    public void Return()
    {
        // Calculate the direction from the hook to the player
        Vector2 directionToPlayer = (player.position - hook.position).normalized;

        hook.velocity = Vector2.zero;

        // Move the hook towards the player
        hook.position += directionToPlayer * Time.deltaTime * returnForce;

        // Check if the hook is within a certain range to the player
        if (Vector2.Distance(player.position, hook.position) < hookReturnRange)
        {
            Disable();
        }
    }

    void Disable()
    {
        transform.parent = player.transform;
        transform.position = player.position + (Vector2)player.transform.right * 0.2f;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        currentHookState = HookState.Idle;
        onHookStateChanged?.Invoke(currentHookState);

        hook.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        RotateHook();
        hookLine.SetPosition(0, player.position);
        hookLine.SetPosition(1, transform.position);

        distanceFromPlayer = player.position - (Vector2)transform.position;

        distanceFromTarget = hookTargetPosition - (Vector2)transform.position;

        if ((distanceFromPlayer.magnitude > hookRange || distanceFromTarget.magnitude <= .5f) && currentHookState == HookState.Out)
        {
            currentHookState = HookState.Pull;
            onHookStateChanged?.Invoke(currentHookState);
        }

        if (currentHookState == HookState.Pull)
        {
            Pull();
        }

        if (currentHookState == HookState.Return)
        {
            Return();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentHookState == HookState.Out)
        { 
            currentHookState = HookState.Pull;
            onHookStateChanged?.Invoke(currentHookState);
        }
            
    }

    void RotateHook()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationHookSpeed * Time.deltaTime);
    }
}
