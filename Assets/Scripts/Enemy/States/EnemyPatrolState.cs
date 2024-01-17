using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrolState : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public LayerMask whatIsGround, whatIsObstacle;

    private bool isFacingRight = true;
    public float rayCastDistance;
    public float obstacleDistance;
    public float enemySpeed;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, rayCastDistance, whatIsGround);
        RaycastHit2D obstacleHit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, obstacleDistance, whatIsObstacle);
        if (groundHit.collider == null || obstacleHit.collider == true)
        {
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        if(isFacingRight)
        {
            enemyRb.velocity = new Vector2(enemySpeed, enemyRb.velocity.y);
        }
        else
        {
            enemyRb.velocity = new Vector2(-enemySpeed, enemyRb.velocity.y);
        }
    }

    private void Rotate()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }
}
