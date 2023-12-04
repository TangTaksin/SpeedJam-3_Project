using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoint : MonoBehaviour
{
    [SerializeField] int maxHitPoint = 3;
    [SerializeField] float iFrameTime = 3f;
    int hitPoint;
    bool isInvulnerable = false;
    [SerializeField] Animator invulAnim;

    public delegate void OnHealthChanged(int newHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    public delegate void OnInvulnerable(bool isInvul);
    public event OnInvulnerable onInvulnerable;

    private void Start()
    {
        hitPoint = maxHitPoint;
        onHealthChanged?.Invoke(hitPoint, maxHitPoint);
    }

    public void ReduceHP(int amount)
    {
        if (!isInvulnerable)
        { 
            hitPoint -= amount;
            onHealthChanged?.Invoke(hitPoint, maxHitPoint);
            StartCoroutine(StartInvulTimer());
        }
    }

    public void SetIsInvulnerable(bool state)
    {
        isInvulnerable = state;
        invulAnim?.gameObject.SetActive(isInvulnerable);
        onInvulnerable?.Invoke(isInvulnerable);
    }

    public IEnumerator StartInvulTimer()
    {
        SetIsInvulnerable(true);

        yield return new WaitForSeconds(iFrameTime);

        SetIsInvulnerable(false);
    }
}
