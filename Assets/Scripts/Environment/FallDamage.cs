using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    //private PlayerHealth playerHealth;
    //private void fallDamageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            //playerHealth.damage(fallDamageAmount);
        }
    }
}
