using System.Collections;
using UnityEngine;

public class Hitpoint : MonoBehaviour
{
    [SerializeField] private int maxHitPoint = 3;
    [SerializeField] private float iFrameTime = 3f;
    private int hitPoint;
    private bool isInvulnerable = false;

    [SerializeField] private Animator invulAnim;

    public delegate void OnHealthChanged(int newHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    public delegate void OnInvulnerable(bool isInvul);
    public event OnInvulnerable onInvulnerable;

    private void Start()
    {
        hitPoint = maxHitPoint;
        UpdateHealth();
    }

    public void ReduceHP(int amount)
    {
        if (!isInvulnerable)
        {
            hitPoint = Mathf.Max(0, hitPoint - amount);
            UpdateHealth();
            StartCoroutine(StartInvulTimer());
        }
    }

    public void SetIsInvulnerable(bool state)
    {
        isInvulnerable = state;
        invulAnim?.gameObject.SetActive(isInvulnerable);
        onInvulnerable?.Invoke(isInvulnerable);
    }

    private void UpdateHealth()
    {
        onHealthChanged?.Invoke(hitPoint, maxHitPoint);
    }

    private IEnumerator StartInvulTimer()
    {
        SetIsInvulnerable(true);
        yield return new WaitForSeconds(iFrameTime);
        SetIsInvulnerable(false);
    }
}
