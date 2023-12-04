using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Hitpoint enemyhp;
    protected Rigidbody2D enemyBody;
    protected SpriteRenderer enemyRenderer;

    [Header("Health")]
    [SerializeField] Gradient healthStateColor;

    [Header("Score")]
    [SerializeField] float hitScore = 100;
    [SerializeField] float killScore = 200;
    [SerializeField] float knockbackForce = 10;
    [SerializeField] ParticleSystem Explosion;

    [Header("Movement")]
    [SerializeField] protected float movementSpeed;

    public delegate void OnHit(float score);
    public static event OnHit onHit;
    public static event OnHit onKill;

    protected Transform player;

    private void OnEnable()
    {
        //find player
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyBody = GetComponent<Rigidbody2D>();
        enemyhp = GetComponent<Hitpoint>();

        if (enemyhp != null)
        {
            enemyhp.onHealthChanged += onHealthChanged;
        }
    }

    void onHealthChanged(int health, int maxHitPoint)
    {
        var colorLevel = 1 - ((float)health / (float)maxHitPoint);
        enemyRenderer.color = healthStateColor.Evaluate(colorLevel);

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

    private void Update()
    {
        //Adjust Rotation base on velocity
       
        float angle = Mathf.Atan2(enemyBody.velocity.y, enemyBody.velocity.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle -90));
        transform.rotation = rotation;

    }

    private void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        //get direction
        var direction = player.position - transform.position;
        direction.Normalize();

        //move towards player
        enemyBody.AddForce(direction * movementSpeed);
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
