using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<Vector2> damageableHit;
    
    Animator animator;

    //[SerializeField] private bool _isAlive = true;

    public bool IsAlive 
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
        set 
        {
            //_isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        IsAlive = true;
    }

    void Update()
    {

    }

    // Returns if the Damageable took damage or not
    public bool Hit(Vector2 knockback)
    {
        // Successful hit
        if (IsAlive)
        {
            Debug.Log("[Damageable] Hit");

            // Kill
            IsAlive = false;

            // Notify other subscribed components that the damageable was hit, so they handle the knockback and such
            animator.SetBool(AnimationStrings.gotHit, true);
            damageableHit?.Invoke(knockback);

            return true;
        }
        // Unable to be hit
        else
        {
            Debug.Log("[Damageable] Unable to hit");
            return false;
        }
    }
}
