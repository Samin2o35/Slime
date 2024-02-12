using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : MonoBehaviour
{
    public Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, obstacleLayer, playerLayer;

    private bool facingRight = true;
    public bool playerDetected;

    public float groundDistance, obstacleDistance, playerDetectDistance;
    public float enemyMoveSpeed;
    public float detectionPauseTime;
    public GameObject alert;

    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if(!playerDetected)
        {
            if (facingRight)
            {
                enemyRb.velocity = new Vector2(enemyMoveSpeed, enemyRb.velocity.y);
            }
            else
            {
                enemyRb.velocity = new Vector2(-enemyMoveSpeed, enemyRb.velocity.y);
            }
        }
    }

    private void Update()
    {
        CheckForTerrain();
        CheckForPlayer();
    }

    private void CheckForTerrain()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, 
            groundDistance, groundLayer);

        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingRight?
            Vector2.right : Vector2.left, obstacleDistance, obstacleLayer);

        if (hit.collider == null || hitObstacle.collider == true)
        {
            Rotate();
        }
    }

    private void CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingRight? 
            Vector2.right : Vector2.left ,playerDetectDistance, playerLayer);

        if (hitPlayer.collider == true)
        {
            StartCoroutine(PlayerDetected());
        }
        else if(playerDetected)
        {
            StartCoroutine(PlayerNotDetected());
        }
    }

    IEnumerator PlayerDetected()
    {
        playerDetected = true;
        enemyRb.velocity = Vector2.zero;
        alert.SetActive(true);
        yield return new WaitForSeconds(detectionPauseTime);
    }

    IEnumerator PlayerNotDetected()
    {
        yield return new WaitForSeconds(detectionPauseTime);
        playerDetected = false;
        alert.SetActive(false);
    }

    private void Rotate()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    #region Debugging Region
    private void OnDrawGizmos()
    {
        // ground check
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * groundDistance);

        // player check
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ledgeDetector.position, (facingRight ? Vector2.right : Vector2.left) 
            * playerDetectDistance);

        // obstacle check
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (facingRight ? Vector2.right : Vector2.left)
            * obstacleDistance);
    }
    #endregion
}
