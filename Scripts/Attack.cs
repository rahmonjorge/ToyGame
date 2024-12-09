using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{

    public Vector2 knockback = Vector2.zero;

    public bool isFront;

    [SerializeField]
    private float parentDirection;

    [SerializeField]
    private int knockbackDirection;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        if (knockbackDirection != 0) knockbackDirection = GetKnockbackDirection();
    }

    private int GetKnockbackDirection()
    {
        parentDirection = transform.parent.transform.localScale.x;

        if (isFront) // Knockback in the same direction parent is facing
        {
            return (int) parentDirection;
        }
        else // Knockback in the opposite direction parent is facing
        {
            return (int) parentDirection * -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {            
            knockback.x *= knockbackDirection;

            // Hit the target
            bool gotHit = damageable.Hit(knockback);
            if (gotHit) Debug.Log("[Attack] " + collision.name + " Attack was successful");
            else Debug.Log("[Attack] Attack failuire");
        }
    }
}
