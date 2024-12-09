using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Animator))]

public class EnemyController : MonoBehaviour
{
    public UnityEvent enemyDies;

    public float walkSpeed = 0f;

    public string weakness = "Object";

    public string weaknessDirection = "Top";

    public Vector2 startPos;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left }

    [SerializeField]
    private WalkableDirection _walkDirection;
    [SerializeField]
    private Vector2 walkDirectionVector;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }

                _walkDirection = value;
            }

        }
    }

    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
        set { animator.SetBool(AnimationStrings.isAlive, value); }
    }

    public bool CanRespawn
    {
        get { return animator.GetBool(AnimationStrings.canRespawn); }
        set { animator.SetBool(AnimationStrings.canRespawn, value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        startPos = rb.transform.position;
    }

    void Start()
    {
        transform.position = startPos;
        IsAlive = true;
        CanRespawn = false;
        walkDirectionVector = Vector2.right;

        animator.SetFloat(AnimationStrings.walkSpeed, walkSpeed);
        //GotHit = false;
    }

    private void FixedUpdate()
    {
        if (IsAlive)
        {
            if (touchingDirections.IsGrounded && touchingDirections.IsOnWall) FlipDirection();

            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
            
        }

        if (CanRespawn) // Wait until death animation finishes
        {
            Respawn(); 
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        } else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } else
        {
            Debug.LogError("Current walk direction is not set to legal values of right or left");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect the layer of the object that collided
        int collidedLayer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(collidedLayer);

        // Determine the direction of the collision
        Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;
        collisionDirection.Normalize(); // Normalize to get the direction vector

        // Log or handle the layer and direction
        //Debug.Log($"Collided with object on layer {layerName}");
        //Debug.Log($"Collision direction: {collisionDirection}");

        switch (weaknessDirection)
        {
            case "Top":
                if (collisionDirection.y > 0 && layerName.Equals(weakness))
                {
                    damageable.Hit(collisionDirection);
                }
            break;
            case "Sides":
                // do something
            break;
            default:
                Debug.LogError("[EnemyController] Error when detecting collision direction and weakness");
            break;
        }
        
    }


    public void OnHit(Vector2 knockback)
    {
        
    }

    public void Respawn()
    {
        enemyDies.Invoke();

        //Awake();
        if (walkSpeed != 0) 
        {
            walkSpeed++;
        }
        
        Start();
    }
}