using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerContoller : MonoBehaviour
{
    public UnityEvent playerDies;

    public Vector2 startPos = Vector2.zero;
    public float jumpImpulse = 8f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Rigidbody2D rb;
    Animator animator;
    public float baseSpeed = 5f;
    public float airSpeed = 4f;

    [SerializeField]
    private GameObject[] children;

    [SerializeField]
    private GameObject currentObject;

    [SerializeField]
    private int currentIndex;

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        return baseSpeed;
                    }
                    else
                    {
                        // Air move
                        return airSpeed;
                    }
                }
                else return 0;
            }
            else return 0;
        }
    }

    private bool _isMoving = false;

    public bool IsMoving 
    { 
        get { return _isMoving; } 
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set {
            // Flip only if value is new
            if (_isFacingRight != value)
            {
                // Flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        } 
    }

    public bool CanMove
    { 
        get {return animator.GetBool(AnimationStrings.canMove);} 
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

    public bool GotHit
    {
        get { return animator.GetBool(AnimationStrings.gotHit); }
        set { animator.SetBool(AnimationStrings.gotHit, value); }
    }

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();

        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) // Index starts at 1 because I don't want to get the indicator
        {
            children[i] = transform.GetChild(i).gameObject;
        }
    }

    void Start()
    {
        transform.position = startPos;
        IsAlive = true;
        CanRespawn = false;
        GotHit = false;
    }

    void FixedUpdate() 
    {

        if (!LockVelocity && IsAlive)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        if (!IsAlive) 
        {
            rb.velocity = Vector2.zero;
        }
        

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if (CanRespawn) // Wait until death animation finishes
        {
            Respawn();
        }
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsAlive && context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            if (children == null || children.Length == 0) return;

            // Deactivate the current object
            if (currentObject != null)
            {
                currentObject.SetActive(false);


                
            }

            // Update the index to the next object
            currentIndex = (currentIndex + 1) % children.Length;

            // Set the new current object and activate it
            currentObject = children[currentIndex];
            currentObject.SetActive(true);
        }
    }

    public void OnHit(Vector2 knockback)
    {
        Debug.Log("[Player] Hit");

        //LockVelocity = true;
        //rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void Respawn()
    {
        playerDies.Invoke();
        Awake();
        Start();
    }
}
