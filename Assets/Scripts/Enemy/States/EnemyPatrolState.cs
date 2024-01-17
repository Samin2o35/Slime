using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBaseState : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public Transform enemyPos;
    public GameObject alert;

    [Header("Check Environment")]
    public LayerMask whatIsGround;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsPlayer;

    [Header("Check Distance")]
    public float rayCastDistance;
    public float obstacleDistance;
    public float playerDetectDistance;

    [Header("Enemy Variables")]
    public float enemySpeed;
    public float playerDetectPauseTime;

    [Header("Booleans")]
    private bool isFacingRight = true;
    private bool isPlayerDetected; 

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckForTerrain();
        CheckForPlayer();
    }

    private void FixedUpdate()
    {
        if (!isPlayerDetected) 
        {
            if (isFacingRight)
            {
                enemyRb.velocity = new Vector2(enemySpeed, enemyRb.velocity.y);
            }
            else
            {
                enemyRb.velocity = new Vector2(-enemySpeed, enemyRb.velocity.y);
            }
        }
    }

    private void CheckForTerrain()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, rayCastDistance, whatIsGround);
        RaycastHit2D obstacleHit = Physics2D.Raycast(ledgeDetector.position, Vector2.right, obstacleDistance, whatIsObstacle);
        
        if (groundHit.collider == null || obstacleHit.collider == true)
        {
            Rotate();
        }
    }

    private void CheckForPlayer()
    {
        RaycastHit2D playerDetectHitLeft = Physics2D.Raycast(enemyPos.position, Vector2.left, playerDetectDistance, whatIsPlayer);
        RaycastHit2D playerDetectHitRight = Physics2D.Raycast(enemyPos.position, Vector2.right, playerDetectDistance, whatIsPlayer);
        
        if (playerDetectHitLeft.collider == true || playerDetectHitRight.collider == true)
        {
            StartCoroutine(PlayerDetected());
        }
        // Only call when player is out of range
        else if(isPlayerDetected)
        {
            StartCoroutine(PlayerNotDetected());
        }
    }

    IEnumerator PlayerDetected()
    {
        isPlayerDetected = true;
        enemyRb.velocity = Vector2.zero;
        alert.SetActive(true);
        yield return new WaitForSeconds(playerDetectPauseTime);
        Debug.Log("I'mma kill you");
    }

    IEnumerator PlayerNotDetected()
    {
        yield return new WaitForSeconds(playerDetectPauseTime);
        isPlayerDetected = false;
        alert.SetActive(false);
    }

    private void Rotate()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyPos.position, playerDetectDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * rayCastDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (isFacingRight ? Vector2.right : Vector2.left) * obstacleDistance);
    }
}
