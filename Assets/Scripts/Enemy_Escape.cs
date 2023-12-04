using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Escape : Enemy
{
    [SerializeField] float detectionRange;

    protected override void Movement()
    {
        var distanceToPlayer = Vector2.Distance(player.position, transform.position);

        var direction = player.position - transform.position;
        direction.Normalize();

        if (distanceToPlayer <= detectionRange)
        {
            enemyBody.AddForce(-direction * movementSpeed);
        }
    }
}
