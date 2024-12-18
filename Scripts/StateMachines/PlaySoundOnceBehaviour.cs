using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnceBehaviour : StateMachineBehaviour
{
    public AudioClip sound;
    public float volume = 1f;

    public bool playOnEnter = true, playonExit = false, playAfterDelay = false;


    // Delayed sound effect
    public float delay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(sound, animator.gameObject.transform.position, volume);
        }

        timeSinceEntered = 0;
        hasDelayedSoundPlayed = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayed)
        {
            //https://youtu.be/QcP_KLeW2VU?list=PLyH-qXFkNSxmDU8ddeslEAtnXIDRLPd_V&t=477 <-------- já ta com o tempo do video só voltar lá e ver
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playonExit)
        {
            AudioSource.PlayClipAtPoint(sound, animator.gameObject.transform.position, volume);
        }
    }

}
