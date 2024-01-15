using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy
{
    [Header("Patrol")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] private float circleRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField]  private float moveSpeed;
    private float moveDirection;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
