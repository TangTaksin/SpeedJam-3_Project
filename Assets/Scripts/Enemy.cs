using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Hitpoint enemyhp;

    [SerializeField] float hitScore = 100;
    [SerializeField] float killScore = 200;
    [SerializeField] float knockbackForce = 10;
    [SerializeField] ParticleSystem Explosion;

    public delegate void OnHit(float score);
    public static event OnHit onHit;
    public static event OnHit onKill;

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
            onKill?.Invoke(killScore);
            StartCoroutine(onDeath());
            return;
        }
        onHit?.Invoke(hitScore);
    }

    public void reactivate()
    {
        gameObject.SetActive(true);
    }

    IEnumerator onDeath()
    {
        Explosion?.gameObject.SetActive(true);
        Explosion?.Play();
        yield return new WaitForSeconds(.5f);
        Explosion?.gameObject.SetActive(false);
        gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent<Hitpoint>(out Hitpoint playerHp);
            collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerbody);

            playerHp.ReduceHP(1);

            //apply knockback if possible
            var deflectDirection = collision.transform.position - transform.position;
            deflectDirection.Normalize();

            playerbody.AddForce(deflectDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
