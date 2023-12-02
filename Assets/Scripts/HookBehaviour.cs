using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    [SerializeField] float hookSpeed = 5f;
    [SerializeField] float hookRange = 5f;
    [SerializeField] float hookReturnRange = 1f;
    [SerializeField] float pullPower = 1f;
    [SerializeField] float returnForce = 1f;
    [SerializeField] LayerMask grabableLayer;

    Rigidbody2D player;
    Rigidbody2D hook;
    LineRenderer hookLine;

    Vector2 hookTargetPosition;
    Vector2 distanceFromPlayer;
    Vector2 distanceFromTarget;

    enum HookState { Out, Pull, Return, Idle }
    HookState currentHookState = HookState.Idle;

    public delegate void OnHookFinished();
    public event OnHookFinished onHookFinished;

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
        }

        if (currentHookState == HookState.Idle)
        {
            transform.parent = null;
            currentHookState = HookState.Out;

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
        hook.position += directionToPlayer * Time.deltaTime * returnForce ;

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
        hook.bodyType = RigidbodyType2D.Kinematic;

        onHookFinished.Invoke();
    }

    private void Update()
    {
        hookLine.SetPosition(0, player.position);
        hookLine.SetPosition(1, transform.position);

        distanceFromPlayer = player.position - (Vector2)transform.position;

        distanceFromTarget = hookTargetPosition - (Vector2)transform.position;

        if ((distanceFromPlayer.magnitude > hookRange || distanceFromTarget.magnitude <= .5f) && currentHookState == HookState.Out)
            currentHookState = HookState.Pull;

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
            currentHookState = HookState.Pull;
    }
}
