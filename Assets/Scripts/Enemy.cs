using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Hitpoint enemyhp;

    [SerializeField] float hitScore = 100;
    [SerializeField] float killScore = 200;

    public delegate void OnHit(float score);
    public static event OnHit onHit;

    private void Start()
    {
        enemyhp = GetComponent<Hitpoint>();

        if (enemyhp != null)
        {
            enemyhp.onHealthChanged += onHealthChanged;
        }
    }

    void onHealthChanged(int health)
    {
        if (health <= 0)
        {
            onHit?.Invoke(killScore);
            return;
        }
        onHit?.Invoke(hitScore);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent<Hitpoint>(out Hitpoint playerHp);
            collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerbody);

            playerHp.ReduceHP(1);

            //apply knockback if possible
        }
    }
}
