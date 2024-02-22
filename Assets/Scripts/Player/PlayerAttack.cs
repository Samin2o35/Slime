using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    public float knockbackForce;

    public Vector2 knockbackAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            int facinfDirection = transform.position.x > 
                collision.transform.position.x ? -1 : 1;

            damageable.Damage(damage, knockbackForce, new Vector2(knockbackAngle.x 
                * facinfDirection, knockbackAngle.y));
        }
    }
}
