using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : MonoBehaviour
{
    public Rigidbody2D enemyRb;
    public float enemyMoveSpeed;

    public Transform ledgeDetector;
    public LayerMask groundLayer, obstacleLayer;
    public float raycastDistance, obstacleDistance;

    private void Start()
    {
        
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, 
            raycastDistance, groundLayer);
        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, Vector2.down,
            obstacleDistance, obstacleLayer);

        if (hit.collider == null || hitObstacle.collider == null)
        {
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        enemyRb.velocity = new Vector2(enemyMoveSpeed, enemyRb.velocity.y);
    }

    private void Rotate()
    {
        transform.Rotate(0, 180, 0);
    }
}
