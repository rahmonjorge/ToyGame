using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses the CapsuleCollider2D component to check if current object is touching ground, wall or ceiling, and from which direction.
public class TouchingDirections : MonoBehaviour
{  
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.1f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCollider;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded { get { 
            return _isGrounded; 
        } private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        } 
    }
    
    [SerializeField]
    private bool _isOnWall;
    
    public bool IsOnWall
    { 
        get 
        {
            return _isOnWall;
        } 
        private set 
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        } 
    }

    [SerializeField]
    private bool _isOnCeiling;
    
    private Vector2 wallCheckDirection
    {
        get
        {
            if (gameObject.transform.localScale.x > 0)
            {
                return Vector2.right;
            }
            else if (gameObject.transform.localScale.x < 0)
            {
                return Vector2.left;
            }
            else
            {
                Debug.LogError("Game object's transform local scale X is set to zero");
                return Vector2.zero;
            }
        }
    }

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // FixedUpdate is called once in a fixed period of time
    void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
