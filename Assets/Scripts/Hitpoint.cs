using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoint : MonoBehaviour
{
    [SerializeField] int maxHitPoint = 3;
    [SerializeField] float iFrameTime = 3f;
    int hitPoint;
    bool cantakedamage = true;

    public delegate void OnHealthChanged(int newHealth);
    public event OnHealthChanged onHealthChanged;

    public delegate void OnInvulnerable(bool isInvul);
    public event OnInvulnerable onInvulnerable;

    private void Start()
    {
        hitPoint = maxHitPoint;
        onHealthChanged?.Invoke(hitPoint);
    }

    public void ReduceHP(int amount)
    {
        if (cantakedamage)
        { 
            hitPoint -= amount;
            onHealthChanged?.Invoke(hitPoint);
            StartCoroutine(StartInvulTimer());
        }
    }

    public void SetIsInvulnerable(bool state)
    {
        cantakedamage = !state;
        onInvulnerable?.Invoke(cantakedamage);
    }

    public IEnumerator StartInvulTimer()
    {
        cantakedamage = false;
        onInvulnerable?.Invoke(cantakedamage);

        yield return new WaitForSeconds(iFrameTime);

        cantakedamage = true;
        onInvulnerable?.Invoke(cantakedamage);
    }
}
